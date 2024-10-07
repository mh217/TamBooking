using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Guid> InputAddressAsync(Address address)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string commandText = @"
                INSERT INTO ""Address"" (""Id"", ""Line"", ""Suite"", ""BuildingNumber"", ""DateCreated"", ""DateUpdated"", ""CreatedByUserId"", ""UpdatedByUserId"", ""IsActive"", ""TownId"") VALUES
                (@id, @line, @suite, @buildingNumber, @dateCreated, @dateUpdated, @createdByUserId, @updatedByUserId, @isActive, @townId) RETURNING ""Id"" ";

            connection.Open();

            using var command = new NpgsqlCommand(commandText, connection);

            command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
            command.Parameters.AddWithValue("@line", address.Line);
            command.Parameters.AddWithValue("@suite", address.Suite);
            command.Parameters.AddWithValue("@buildingNumber", address.BuildingNumber);
            command.Parameters.AddWithValue("@dateCreated", address.DateCreated);
            command.Parameters.AddWithValue("@dateUpdated", address.DateUpdated);
            command.Parameters.AddWithValue("@createdByUserId", address.CreatedByUserId);
            command.Parameters.AddWithValue("@updatedByUserId", address.UpdatedByUserId);
            command.Parameters.AddWithValue("@isActive", address.IsActive);
            command.Parameters.AddWithValue("@townId", address.TownId);

            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            var addressId = Guid.Empty;

            if (!reader.HasRows)
            {
                throw new Exception();
            }

            await reader.ReadAsync();
            addressId = Guid.Parse(reader["Id"].ToString());
            connection.Close();
            return addressId;
        }
    }
}