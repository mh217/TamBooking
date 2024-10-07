using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class CountyRepository : ICountyRepository
    {
        private readonly string _connectionString;

        public CountyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<County>> GetAllCountiesAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT * FROM \"County\" WHERE \"IsActive\" = TRUE ";
            using var cmd = new NpgsqlCommand(sql, connection);

            using var reader = await cmd.ExecuteReaderAsync();

            List<County> counties = [];

            while (await reader.ReadAsync())
            {
                County county = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId"))
                };
                counties.Add(county);
            }
            return counties;
        }
    }
}