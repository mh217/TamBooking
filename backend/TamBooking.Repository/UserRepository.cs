using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM \"User\" u LEFT JOIN \"Role\" r ON u.\"RoleId\" = r.\"Id\" WHERE \"Email\" = @email ";
            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("email", email);

            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                User user = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    RoleId = reader.GetGuid(reader.GetOrdinal("RoleId")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    Role = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("RoleId")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                    }
                };
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM \"User\" u LEFT JOIN \"Role\" r ON u.\"RoleId\" = r.\"Id\" WHERE u.\"Id\" = @id ";
            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("id", id);

            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                User user = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    RoleId = reader.GetGuid(reader.GetOrdinal("RoleId")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    Role = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("RoleId")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                    }
                };
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<Guid> InsertUserAsync(string email, string password, Guid roleId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "INSERT INTO \"User\" (\"Email\", \"Password\", \"RoleId\") " +
                "VALUES (@email, @password, @roleId) RETURNING \"Id\" ";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("roleId", roleId);

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

        public async Task<bool> ChangeUserPasswordAsync(Guid id, string password)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE \"User\" SET \"Password\" = @password WHERE \"Id\" = @id ";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("id", id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ChangeUserEmailAsync(Guid id, string email)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE \"User\" SET \"Email\" = @email WHERE \"Id\" = @id ";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("id", id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ActivateUser(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE \"User\" SET \"IsActive\" = TRUE WHERE \"Id\" = @id ";
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