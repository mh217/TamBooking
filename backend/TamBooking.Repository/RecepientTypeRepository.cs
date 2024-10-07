using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class RecepientTypeRepository : IRecepientTypeRepository
    {
        private readonly string _connectionString;

        public RecepientTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<RecepientType>> GetRecepientTypesAsync()
        {
            List<RecepientType> types = new List<RecepientType>();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT * FROM \"RecepientType\" WHERE \"IsActive\" = true;";
            var command = new NpgsqlCommand(commandText, connection);

            connection.Open();
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    RecepientType type = new RecepientType();
                    type.Id = Guid.Parse(reader["Id"].ToString());
                    type.Name = reader["Name"].ToString();
                    type.IsActive = (bool)reader["IsActive"];
                    type.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                    type.DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString());
                    type.CreatedByUserId = Guid.Parse(reader["CreatedByUserId"].ToString());
                    type.UpdatedByUserId = Guid.Parse(reader["UpdatedByUserId"].ToString());
                    types.Add(type);
                }
            }
            return types;
        }
    }
}