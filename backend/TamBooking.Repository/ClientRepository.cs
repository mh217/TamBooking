using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateClientAsync(Client client)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "INSERT INTO \"Client\" (\"Id\", \"FirstName\", \"LastName\", \"TownId\") VALUES (@id, @firstName, @lastName, @townId);";
            var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@id", client.Id);
            command.Parameters.AddWithValue("@firstName", client.FirstName);
            command.Parameters.AddWithValue("@lastName", client.LastName);
            command.Parameters.AddWithValue("@townId", client.TownId);

            connection.Open();
            var numberOfCommits = await command.ExecuteNonQueryAsync();
            connection.Close();

            if (numberOfCommits == 0)
            {
                throw new Exception();
            }
        }

        public async Task<Client?> GetClientAsync(Guid id)
        {
            Client client = new Client();
            Town town = new Town();
            County county = new County();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT cl.\"Id\", cl.\"FirstName\", cl.\"LastName\",cl.\"TownId\", tw.\"CountyId\", tw.\"Name\" AS \"TownName\", co.\"Name\" AS \"CountyName\" FROM \"Client\" cl LEFT JOIN \"Town\" tw ON cl.\"TownId\" = tw.\"Id\" LEFT JOIN \"User\"  u ON u.\"IsActive\" = true LEFT JOIN \"County\" co ON tw.\"CountyId\" = co.\"Id\" WHERE cl.\"Id\" = @id;";
            var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                await reader.ReadAsync();
                client.Id = Guid.Parse(reader["Id"].ToString());
                client.FirstName = reader["FirstName"].ToString();
                client.LastName = reader["LastName"].ToString();
                if (reader["CountyName"] != DBNull.Value)
                {
                    county.Id = Guid.Parse(reader["CountyId"].ToString());
                    county.Name = reader["CountyName"].ToString();
                    town.County = county;
                }
                if (reader["TownName"] != DBNull.Value)
                {
                    town.Id = Guid.Parse(reader["TownId"].ToString());
                    town.Name = reader["TownName"].ToString();
                }
                client.Town = town;
            }
            else
            {
                return null;
            }
            return client;
        }

        public async Task<bool> UpdateClientAsync(Guid id, Client client)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE \"Client\" SET");

            var command = new NpgsqlCommand();
            command.Connection = connection;

            if (client.FirstName != null)
            {
                stringBuilder.Append(" \"FirstName\" = @firstName, ");
                command.Parameters.AddWithValue("@firstName", client.FirstName);
            }
            if (client.LastName != null)
            {
                stringBuilder.Append(" \"LastName\" = @lastName, ");
                command.Parameters.AddWithValue("@lastName", client.LastName);
            }
            if (client.TownId != Guid.Empty)
            {
                stringBuilder.Append(" \"TownId\" = @townId, ");
                command.Parameters.AddWithValue("@townId", client.TownId);
            }

            stringBuilder.Length -= 2;
            stringBuilder.Append(" WHERE \"Id\" = @id;");

            command.CommandText = stringBuilder.ToString();
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