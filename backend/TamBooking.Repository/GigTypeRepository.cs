using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class GigTypeRepository : IGigTypeRepository
    {
        private readonly string _connectionString;

        public GigTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<GigType>> GetAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string commandText = $"SELECT * FROM \"GigType\" WHERE \"IsActive\" = true";

            using var command = new NpgsqlCommand(commandText, connection);
            using var reader = await command.ExecuteReaderAsync();

            var gigTypes = new List<GigType>();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var gigType = new GigType
                    {
                        Id = Guid.Parse(reader[0].ToString()),
                        Name = reader[1].ToString(),
                        IsActive = reader.GetBoolean(2),
                        DateCreated = reader.GetDateTime(3),
                        DateUpdated = reader.GetDateTime(4),
                        CreatedByUserId = Guid.Parse(reader[5].ToString()),
                        UpdatedByUserId = Guid.Parse(reader[6].ToString())
                    };

                    gigTypes.Add(gigType);
                }
            }

            connection.Close();
            return gigTypes;
        }
    }
}