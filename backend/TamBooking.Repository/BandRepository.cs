using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;
using TamBooking.Common;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class BandRepository : IBandRepository
    {
        private readonly string _connectionString;

        public BandRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<List<Band>> GetAllBandsCountAsync(BandFilter filter)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT b.*, t.\"CountyId\" AS \"CountyId\" FROM \"Band\" b JOIN \"Town\" t ON b.\"TownId\" = t.\"Id\" WHERE 1=1 ";
            using var command = new NpgsqlCommand();
            var sb = new StringBuilder(commandText);

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                sb.Append($" AND b.\"Name\" ILIKE @searchQuery");
                command.Parameters.AddWithValue("@searchQuery", $"%{filter.SearchQuery}%");
            }
            if (filter.CountyId != null)
            {
                sb.Append($" AND t.\"CountyId\" = @countyId");
                command.Parameters.AddWithValue("@countyId", filter.CountyId == null ? DBNull.Value : filter.CountyId);
            }
            if (filter.Id != null)
            {
                sb.Append($" AND b.\"Id\" = @id");
                command.Parameters.AddWithValue("@id", filter.Id == null ? DBNull.Value : filter.Id);
            }
            if (filter.PriceFrom != 0 && filter.PriceTo != 0)
            {
                sb.Append($" AND b.\"Price\" > @filterPriceFrom AND b.\"Price\" < @filterPriceTo");
                command.Parameters.AddWithValue("@filterPriceFrom", filter.PriceFrom);
                command.Parameters.AddWithValue("@filterPriceTo", filter.PriceTo);
            }
            if (filter.PriceFrom != 0 && filter.PriceTo == 0)
            {
                sb.Append($" AND b.\"Price\" > @filterPriceFrom");
                command.Parameters.AddWithValue("@filterPriceFrom", filter.PriceFrom);
            }
            if (filter.PriceFrom == 0 && filter.PriceTo != 0)
            {
                sb.Append($" AND b.\"Price\" < @filterPriceTo");
                command.Parameters.AddWithValue("@filterPriceTo", filter.PriceTo);
            }

            command.Connection = connection;
            command.CommandText = sb.ToString();
            connection.Open();

            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            List<Band> bands = new List<Band>();

            if (reader.HasRows)
            {

                while (await reader.ReadAsync())
                {
                    var band = new Band();
                    band.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                    band.Name = reader.GetString(reader.GetOrdinal("Name"));
                    band.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                    band.DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated"));
                    band.DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated"));
                    bands.Add(band);

                }
            }
            return bands;

        }


        public async Task CreateBandAsync(Band band)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "INSERT INTO \"Band\" (\"Id\", \"Name\", \"Price\", \"TownId\") VALUES (@id, @name, @price, @townId);";
            var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@id", band.Id);
            command.Parameters.AddWithValue("@name", band.Name);
            command.Parameters.AddWithValue("@price", band.Price);
            command.Parameters.AddWithValue("@townId", band.TownId);

            connection.Open();
            var numberOfCommits = await command.ExecuteNonQueryAsync();
            connection.Close();

            if (numberOfCommits == 0)
            {
                throw new Exception();
            }
        }


        public async Task<List<Band>> GetAllBandsAsync(BandFilter filter, Paging paging, Sorting sorting)
        {
            List<Band> bands = new List<Band>();
            using var connection = new NpgsqlConnection(_connectionString);
            var commandText = @"SELECT
                                b.""Id"" AS ""BandId"",
                                b.""Name"" AS ""BandName"",
                                b.""Price"" AS ""BandPrice"",
                                b.""DateCreated"" AS ""BandDateCreated"",
                                b.""DateUpdated"" AS ""BandDateUpdated"",
                                t.""Id"" AS ""TownId"",
                                t.""Name"" AS ""TownName"",
                                t.""Zip"" AS ""TownZip"",
                                t.""IsActive"" AS ""TownIsActive"",
                                t.""DateCreated"" AS ""TownDateCreated"",
                                t.""DateUpdated"" AS ""TownDateUpdated"",
                                t.""CountyId"" AS ""CountyId"",
                                u.""Id"" AS ""UserId"",
                                u.""Email"" AS ""UserEmail"",
                                u.""Password"" AS ""UserPassword"",
                                u.""IsActive"" AS ""UserIsActive"",
                                u.""DateCreated"" AS ""UserDateCreated"",
                                u.""DateUpdated"" AS ""UserDateUpdated"",
                                u.""RoleId"" AS ""RoleId""
                            FROM
                                ""Band"" b
                            JOIN
                                ""Town"" t ON b.""TownId"" = t.""Id""
                            JOIN
                                ""User"" u ON b.""Id"" = u.""Id""
                            WHERE
                                u.""IsActive"" = true AND t.""IsActive"" = true
                            ";

            using var command = new NpgsqlCommand();

            var sb = new StringBuilder(commandText);
            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                sb.Append($" AND b.\"Name\" ILIKE @searchQuery");
                command.Parameters.AddWithValue("@searchQuery", $"%{filter.SearchQuery}%");
            }
            if (filter.CountyId != null)
            {
                sb.Append($" AND t.\"CountyId\" = @countyId");
                command.Parameters.AddWithValue("@countyId", filter.CountyId == null ? DBNull.Value : filter.CountyId);
            }
            if (filter.Id != null)
            {
                sb.Append($" AND b.\"Id\" = @id");
                command.Parameters.AddWithValue("@id", filter.Id == null ? DBNull.Value : filter.Id);
            }
            if (filter.PriceFrom != 0 && filter.PriceTo != 0)
            {
                sb.Append($" AND b.\"Price\" > @filterPriceFrom AND b.\"Price\" < @filterPriceTo");
                command.Parameters.AddWithValue("@filterPriceFrom", filter.PriceFrom);
                command.Parameters.AddWithValue("@filterPriceTo", filter.PriceTo);
            }
            if (filter.PriceFrom != 0 && filter.PriceTo == 0)
            {
                sb.Append($" AND b.\"Price\" > @filterPriceFrom");
                command.Parameters.AddWithValue("@filterPriceFrom", filter.PriceFrom);
            }
            if (filter.PriceFrom == 0 && filter.PriceTo != 0)
            {
                sb.Append($" AND b.\"Price\" < @filterPriceTo");
                command.Parameters.AddWithValue("@filterPriceTo", filter.PriceTo);
            }

            if (sorting != null)
            {
                sb.Append($" ORDER BY \"{sorting.OrderBy}\" {sorting.OrderDirection}");
            }

            if (paging != null)
            {
                sb.Append($" LIMIT @rpp OFFSET @offset");
                command.Parameters.AddWithValue("@rpp", paging.Rpp);
                command.Parameters.AddWithValue("offset", paging.Rpp * (paging.PageNumber - 1));
            }

            command.Connection = connection;
            command.CommandText = sb.ToString();

            connection.Open();
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var band = new Band();
                    band.Id = reader.GetGuid(reader.GetOrdinal("BandId"));
                    band.Name = reader.GetString(reader.GetOrdinal("BandName"));
                    band.Price = reader.GetDecimal(reader.GetOrdinal("BandPrice"));
                    band.DateCreated = reader.GetDateTime(reader.GetOrdinal("BandDateCreated"));
                    band.DateUpdated = reader.GetDateTime(reader.GetOrdinal("BandDateUpdated"));

                    band.Town = new Town();
                    band.Town.Id = Guid.Parse(reader["TownId"].ToString());
                    band.Town.Name = reader.GetString(reader.GetOrdinal("TownName"));
                    band.Town.Zip = reader.GetString(reader.GetOrdinal("TownZip"));
                    band.Town.IsActive = reader.GetBoolean(reader.GetOrdinal("TownIsActive"));
                    band.Town.DateCreated = reader.GetDateTime(reader.GetOrdinal("TownDateCreated"));
                    band.Town.DateUpdated = reader.GetDateTime(reader.GetOrdinal("TownDateUpdated"));
                    band.Town.CountyId = reader.GetGuid(reader.GetOrdinal("CountyId"));

                    band.User = new User();
                    band.User.Id = reader.GetGuid(reader.GetOrdinal("UserId"));
                    band.User.Email = reader.GetString(reader.GetOrdinal("UserEmail"));
                    band.User.Password = reader.GetString(reader.GetOrdinal("UserPassword"));
                    band.User.IsActive = reader.GetBoolean(reader.GetOrdinal("UserIsActive"));
                    band.User.DateCreated = reader.GetDateTime(reader.GetOrdinal("UserDateCreated"));
                    band.User.DateUpdated = reader.GetDateTime(reader.GetOrdinal("UserDateUpdated"));
                    band.User.RoleId = reader.GetGuid(reader.GetOrdinal("RoleId"));

                    bands.Add(band);
                }
            }

            connection.Close();

            return bands;
        }

        public async Task<bool> DeleteBandAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var commandText = @"UPDATE ""User""
                            SET ""IsActive"" = false
                            WHERE ""Id"" = @userId;";
            using var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@userId", id);

            connection.Open();
            int numberOfCommits = await command.ExecuteNonQueryAsync();

            if (numberOfCommits == 0)
            {
                return false;
            }

            connection.Close();
            return true;
        }

        public async Task<bool> UpdateBandAsync(Guid id, Band band)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var commandText = @"UPDATE ""Band""
                            SET ""Name"" = @name, ""Price"" = @price, ""TownId"" = @townId
                            WHERE ""Id"" = @userId;";
            using var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@userId", id);
            command.Parameters.AddWithValue("@name", band.Name);
            command.Parameters.AddWithValue("@price", band.Price);
            command.Parameters.AddWithValue("@townId", band.TownId);

            connection.Open();
            int numberOfCommits = await command.ExecuteNonQueryAsync();

            if (numberOfCommits == 0)
            {
                return false;
            }

            connection.Close();
            return true;
        }
    }
}