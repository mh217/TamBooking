using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class TownRepository : ITownRepository
    {
        private readonly string _connectionString;

        public TownRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Town>> GetTownsAsync()
        {
            List<Town> towns = [];
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT town.\"Id\", town.\"Name\", town.\"Zip\", town.\"DateCreated\", town.\"DateUpdated\", town.\"CreatedByUserId\", town.\"UpdatedByUserId\" , town.\"IsActive\", town.\"CountyId\", co.\"Id\", co.\"Name\" FROM \"Town\" town LEFT JOIN \"County\" co ON town.\"CountyId\" = co.\"Id\" WHERE town.\"IsActive\" = true;";
            var command = new NpgsqlCommand(commandText, connection);

            connection.Open();
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Town town = new Town();
                    County county = new County();
                    town.Id = Guid.Parse(reader["Id"].ToString());
                    town.Name = reader["Name"].ToString();
                    town.Zip = reader["Zip"].ToString();
                    town.IsActive = (bool)reader["IsActive"];
                    town.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                    town.DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString());
                    town.CreatedByUserId = Guid.Parse(reader["CreatedByUserId"].ToString());
                    town.UpdatedByUserId = Guid.Parse(reader["UpdatedByUserId"].ToString());
                    town.CountyId = Guid.Parse(reader["CountyId"].ToString());
                    county.Name = reader["Name"].ToString();
                    county.Id = town.CountyId;
                    towns.Add(town);
                }
            }
            return towns;
        }
    }
}