using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;
using TamBooking.Common;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Review>> GetAllReviewsByBandIdAsync(Guid bandId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT * FROM \"Review\" WHERE \"IsActive\" = true AND \"BandId\" = @id;";
            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", bandId);
            connection.Open();

            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            List<Review> reviews = new List<Review>();

            while (await reader.ReadAsync())
            {
                Review review = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                    Text = reader.GetString(reader.GetOrdinal("Text")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                    ClientId = reader.GetGuid(reader.GetOrdinal("ClientId")),
                    BandId = reader.GetGuid(reader.GetOrdinal("BandId")),
                };
                reviews.Add(review);
            }
            return reviews;
        }

        public async Task<List<Review>> GetReviewsAsync(Paging paging, Sorting sorting)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            StringBuilder sql = new StringBuilder(
                @"SELECT r.*, b.""Id"" as BandId, b.""Name"", b.""Price"", b.""DateCreated"" as BandDateCreated, 
                b.""DateUpdated"" as BandDateUpdated, b.""TownId""  
                FROM ""Review"" r
                LEFT JOIN ""Band"" b ON r.""BandId"" = b.""Id""
                WHERE r.""IsActive"" = TRUE ");

            using var cmd = new NpgsqlCommand();
            if (sorting != null)
            {
                sql.Append($"ORDER BY r.\"{sorting.OrderBy}\" {sorting.OrderDirection} ");
            }

            if (paging != null)
            {
                sql.Append($"LIMIT @rpp OFFSET @offset ");
                cmd.Parameters.AddWithValue("rpp", paging.Rpp);
                cmd.Parameters.AddWithValue("offset", paging.Rpp * (paging.PageNumber - 1));
            }

            cmd.Connection = conn;
            cmd.CommandText = sql.ToString();

            using var reader = await cmd.ExecuteReaderAsync();

            List<Review> reviews = [];

            while (await reader.ReadAsync())
            {
                Review review = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                    Text = reader.GetString(reader.GetOrdinal("Text")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                    ClientId = reader.GetGuid(reader.GetOrdinal("ClientId")),
                    BandId = reader.GetGuid(reader.GetOrdinal("BandId")),
                    Band = reader.IsDBNull(reader.GetOrdinal("BandId")) ? null : new Band
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("BandId")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        DateCreated = reader.GetDateTime(reader.GetOrdinal("BandDateCreated")),
                        DateUpdated = reader.GetDateTime(reader.GetOrdinal("BandDateUpdated")),
                        TownId = reader.GetGuid(reader.GetOrdinal("TownId")),
                    }
                };
                reviews.Add(review);
            }
            return reviews;
        }

        public async Task<List<Review>> GetReviewsByBandIdAsync(Guid bandId, Paging paging, Sorting sorting)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            StringBuilder sql = new("SELECT * FROM \"Review\" WHERE \"IsActive\" = TRUE AND \"BandId\" = @bandId ");

            using var cmd = new NpgsqlCommand();
            if (sorting != null)
            {
                sql.Append($"ORDER BY \"{sorting.OrderBy}\" {sorting.OrderDirection} ");
            }

            if (paging != null)
            {
                sql.Append($"LIMIT @rpp OFFSET @offset ");
                cmd.Parameters.AddWithValue("rpp", paging.Rpp);
                cmd.Parameters.AddWithValue("offset", paging.Rpp * (paging.PageNumber - 1));
            }

            cmd.Parameters.AddWithValue("bandId", bandId);
            cmd.Connection = conn;
            cmd.CommandText = sql.ToString();

            using var reader = await cmd.ExecuteReaderAsync();

            List<Review> reviews = [];

            while (await reader.ReadAsync())
            {
                Review review = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                    Text = reader.GetString(reader.GetOrdinal("Text")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                    ClientId = reader.GetGuid(reader.GetOrdinal("ClientId")),
                    BandId = reader.GetGuid(reader.GetOrdinal("BandId")),
                };
                reviews.Add(review);
            }
            return reviews;
        }

        public async Task<Guid> InsertReviewAsync(Review review)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "INSERT INTO \"Review\" (\"Rating\", \"Text\", \"CreatedByUserId\", \"UpdatedByUserId\", \"ClientId\", \"BandId\") " +
                        "VALUES (@rating, @text, @createdByUserId, @updatedByUserId, @clientId, @bandId) " +
                        "RETURNING \"Id\" ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("rating", review.Rating);
            cmd.Parameters.AddWithValue("text", review.Text ?? "");
            cmd.Parameters.AddWithValue("createdByUserId", review.CreatedByUserId);
            cmd.Parameters.AddWithValue("updatedByUserId", review.UpdatedByUserId);
            cmd.Parameters.AddWithValue("clientId", review.ClientId);
            cmd.Parameters.AddWithValue("bandId", review.BandId);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return reader.GetGuid(reader.GetOrdinal("Id"));
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<bool> DeleteReviewByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE \"Review\" SET \"IsActive\" = FALSE WHERE \"Id\" = @id ";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                return false;
            }
            return true;
        }
    }
}