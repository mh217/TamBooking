using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class BandMemberRepository : IBandMemberRepository
    {
        private readonly string _connectionString;

        public BandMemberRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateBandMemberAsync(BandMember member)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "INSERT INTO \"BandMember\" (\"FirstName\", \"LastName\", \"Email\", \"CreatedByUserId\", \"UpdatedByUserId\", \"BandId\") VALUES (@firstName, @lastName, @email, @cratedByUserId, @updatedByUserId, @bandId);";
            var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@firstName", member.FirstName);
            command.Parameters.AddWithValue("@lastName", member.LastName);
            command.Parameters.AddWithValue("@email", member.Email);
            command.Parameters.AddWithValue("@cratedByUserId", member.CreatedByUserId);
            command.Parameters.AddWithValue("@updatedByUserId", member.UpdatedByUserId);
            command.Parameters.AddWithValue("@bandId", member.BandId);

            connection.Open();
            var numberOfCommits = await command.ExecuteNonQueryAsync();
            connection.Close();

            if (numberOfCommits == 0)
            {
                throw new Exception();
            }
        }

        public async Task<List<BandMember>> GetBandMemberAsync(Guid id)
        {
            List<BandMember> members = new List<BandMember>();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT * FROM \"BandMember\" WHERE \"BandId\" = @id AND \"IsActive\" = true";
            using var command = new NpgsqlCommand();

            command.Parameters.AddWithValue("@id", id);

            command.Connection = connection;
            command.CommandText = commandText;

            connection.Open();

            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var bandMember = new BandMember();
                    bandMember.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                    bandMember.Email = reader.GetString(reader.GetOrdinal("Email"));
                    bandMember.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                    bandMember.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                    bandMember.BandId = reader.GetGuid(reader.GetOrdinal("BandId"));

                    members.Add(bandMember);
                }
            }

            connection.Close();

            return members;

        }

        public async Task<bool> DeleteBandMemberAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "UPDATE \"BandMember\" SET \"IsActive\" = false WHERE \"Id\" = @id;";
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