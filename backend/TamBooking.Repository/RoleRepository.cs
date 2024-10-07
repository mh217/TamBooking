
using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM \"Role\" WHERE \"Id\" = @id AND \"IsActive\" = TRUE ";
            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("id", id);

            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Role role = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                };
                return role;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "SELECT * FROM \"Role\" WHERE \"IsActive\" = TRUE AND \"Name\" != 'admin' ";
            using var cmd = new NpgsqlCommand(sql, connection);

            using var reader = await cmd.ExecuteReaderAsync();

            List<Role> roles = [];

            while (await reader.ReadAsync())
            {
                Role role = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                };
                roles.Add(role);
            }
            return roles;
        }
    }
}
