using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class GigRepository : IGigRepository
    {
        private readonly string _connectionString;

        public GigRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Guid> CreateGigAsync(Gig gig)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "INSERT INTO \"Gig\" (\"OccasionDate\", \"TypeId\", \"AddressId\", \"CreatedByUserId\", \"UpdatedByUserId\", " +
                "\"BandId\", \"ClientId\") VALUES (@occasionDate, @typeId, @addressId, @cratedByUserId, @updatedByUserId, @bandId, @clientId) RETURNING \"Id\" ";
            var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@occasionDate", gig.OccasionDate);
            command.Parameters.AddWithValue("@typeId", gig.TypeId);
            command.Parameters.AddWithValue("@addressId", gig.AddressId);
            command.Parameters.AddWithValue("@cratedByUserId", gig.CreatedByUserId);
            command.Parameters.AddWithValue("@updatedByUserId", gig.UpdatedByUserId);
            command.Parameters.AddWithValue("@bandId", gig.BandId);
            command.Parameters.AddWithValue("@clientId", gig.ClientId);

            connection.Open();

            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            var gigId = Guid.Empty;

            if (!reader.HasRows)
            {
                throw new Exception();
            }

            await reader.ReadAsync();
            gigId = Guid.Parse(reader["Id"].ToString());
            connection.Close();
            return gigId;
        }

        public async Task<Gig> GetGigById(Guid id)
        {
            try
            {
                Gig gig = new Gig();
                using var connection = new NpgsqlConnection(_connectionString);
                string commandText = @"SELECT g.*, a.*, t.* FROM ""Gig"" g LEFT JOIN ""Address"" a ON g.""AddressId"" = a.""Id"" LEFT JOIN ""Town"" t ON a.""TownId"" = t.""Id"" WHERE g.""Id"" = @id";

                var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    gig.Id = Guid.Parse(reader["Id"].ToString());
                    gig.OccasionDate = DateTime.Parse(reader["OccasionDate"].ToString());
                    gig.AddressId = Guid.Parse(reader["AddressId"].ToString());
                    gig.ClientId = Guid.Parse(reader["ClientId"].ToString());
                    gig.BandId = Guid.Parse(reader["BandId"].ToString());
                    gig.TypeId = Guid.Parse(reader["TypeId"].ToString());
                    gig.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                    gig.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                    gig.DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString());
                    gig.CreatedByUserId = Guid.Parse(reader["CreatedByUserId"].ToString());
                    gig.UpdatedByUserId = Guid.Parse(reader["UpdatedByUserId"].ToString());

                    gig.Address = new Address
                    {
                        Id = Guid.Parse(reader["AddressId"].ToString()),
                        Line = reader["Line"]?.ToString(),
                        Suite = reader["Suite"]?.ToString(),
                        BuildingNumber = Int32.Parse(reader["BuildingNumber"].ToString()),
                        TownId = reader["TownId"] != DBNull.Value ? Guid.Parse(reader["TownId"].ToString()) : Guid.Empty,
                        Town = new Town
                        {
                            Id = reader["TownId"] != DBNull.Value ? Guid.Parse(reader["TownId"].ToString()) : Guid.Empty,
                            Name = reader["Name"]?.ToString(),
                        }
                    };
                }

                return gig;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public Gig GetGigByIdForNotifications(Guid id)
        {
            Gig gig = new Gig();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = @"SELECT g.*, a.*, t.* FROM ""Gig"" g LEFT JOIN ""Address"" a ON g.""AddressId"" = a.""Id"" LEFT JOIN ""Town"" t ON a.""TownId"" = t.""Id"" WHERE g.""Id"" = @id";

            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            using NpgsqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                gig.Id = Guid.Parse(reader["Id"].ToString());
                gig.OccasionDate = DateTime.Parse(reader["OccasionDate"].ToString());
                gig.AddressId = Guid.Parse(reader["AddressId"].ToString());
                gig.ClientId = Guid.Parse(reader["ClientId"].ToString());
                gig.BandId = Guid.Parse(reader["BandId"].ToString());
                gig.TypeId = Guid.Parse(reader["TypeId"].ToString());
                gig.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                gig.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                gig.DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString());
                gig.CreatedByUserId = Guid.Parse(reader["CreatedByUserId"].ToString());
                gig.UpdatedByUserId = Guid.Parse(reader["UpdatedByUserId"].ToString());

                gig.Address = new Address
                {
                    Id = Guid.Parse(reader["a.Id"].ToString()),
                    Line = reader["a.Line"]?.ToString(),
                    Suite = reader["a.Suite"]?.ToString(),
                    BuildingNumber = Int32.Parse(reader["a.BuildingNumber"].ToString()),
                    TownId = reader["a.TownId"] != DBNull.Value ? Guid.Parse(reader["a.TownId"].ToString()) : Guid.Empty,
                    Town = new Town
                    {
                        Id = reader["t.Id"] != DBNull.Value ? Guid.Parse(reader["t.Id"].ToString()) : Guid.Empty,
                        Name = reader["t.Name"]?.ToString(),
                    }
                };
            }

            return gig;
        }


        public async Task<List<Gig>> GetGigsAsync(Guid id)
        {
            List<Gig> gigs = new List<Gig>();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT gig.\"Id\", gig.\"OccasionDate\", gig.\"TypeId\", gig.\"ClientId\", gig.\"DateCreated\", gig.\"DateUpdated\",  gig.\"CreatedByUserId\", gig.\"UpdatedByUserId\", gig.\"IsActive\", gig.\"BandId\", gig.\"AddressId\", " +
                "ty.\"Id\" AS \"IdType\", ty.\"Name\" AS \"TypeName\", " +
                "cl.\"Id\" AS \"IdClient\", cl.\"FirstName\", cl.\"LastName\"," +
                "bd.\"Id\" AS \"IdBand\", bd.\"Name\" AS \"BandName\", bd.\"Price\", " +
                "address.\"Id\" AS \"IdAddress\", address.\"Line\", address.\"BuildingNumber\"" +
                "FROM \"Gig\" gig " +
                "LEFT JOIN \"GigType\" ty ON gig.\"TypeId\" = ty.\"Id\"" +
                "LEFT JOIN \"Client\" cl ON gig.\"ClientId\" = cl.\"Id\"" +
                "LEFT JOIN \"Band\" bd ON gig.\"BandId\" = bd.\"Id\"" +
                "LEFT JOIN \"Address\" address ON gig.\"AddressId\" = address.\"Id\"" +
                "WHERE gig.\"IsActive\" = true AND gig.\"IsConfirmed\" = true AND (bd.\"Id\" = @id OR cl.\"Id\" = @id) ORDER BY gig.\"OccasionDate\" DESC;";

            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();


            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Gig gig = new Gig();
                    Address address = new Address();
                    Band band = new Band();
                    Client client = new Client();
                    GigType type = new GigType();
                    gig.Id = Guid.Parse(reader["Id"].ToString());
                    gig.OccasionDate = DateTime.Parse(reader["OccasionDate"].ToString());
                    gig.AddressId = Guid.Parse(reader["AddressId"].ToString());
                    gig.ClientId = Guid.Parse(reader["ClientId"].ToString());
                    gig.BandId = Guid.Parse(reader["BandId"].ToString());
                    gig.TypeId = Guid.Parse(reader["TypeId"].ToString());
                    gig.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                    gig.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                    gig.DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString());
                    gig.CreatedByUserId = Guid.Parse(reader["CreatedByUserId"].ToString());
                    gig.UpdatedByUserId = Guid.Parse(reader["UpdatedByUserId"].ToString());

                    client.Id = gig.ClientId;
                    client.FirstName = reader["FirstName"].ToString();
                    client.LastName = reader["LastName"].ToString();
                    gig.Client = client;

                    band.Id = Guid.Parse(reader["BandId"].ToString());
                    band.Name = reader["BandName"].ToString();
                    band.Price = Int32.Parse(reader["Price"].ToString());
                    gig.Band = band;

                    type.Id = Guid.Parse(reader["TypeId"].ToString());
                    type.Name = reader["TypeName"].ToString();
                    gig.GigType = type;

                    address.Id = Guid.Parse(reader["AddressId"].ToString());
                    address.Line = reader["Line"].ToString();
                    address.BuildingNumber = Int32.Parse(reader["BuildingNumber"].ToString());
                    address.Suite = reader["Line"].ToString();
                    gig.Address = address;

                    gigs.Add(gig);
                }
            }
            return gigs;
        }

        public async Task<bool> DeleteGigAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "UPDATE \"Gig\" SET \"IsActive\" = false WHERE \"Id\" = @id;";
            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var numberOfCommits = await command.ExecuteNonQueryAsync();

            if (numberOfCommits == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ConfirmGigAsync(Guid id)
        {
            List<Gig> gigs = new List<Gig>();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "UPDATE \"Gig\" SET \"IsConfirmed\" = true WHERE \"Id\" = @id;";
            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var numberOfCommits = await command.ExecuteNonQueryAsync();

            if (numberOfCommits == 0)
            {
                return false;
            }
            return true;
        }
    }
}