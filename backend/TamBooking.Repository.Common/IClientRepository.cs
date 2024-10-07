using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IClientRepository
    {
        public Task CreateClientAsync(Client client);

        public Task<Client> GetClientAsync(Guid id);

        public Task<bool> UpdateClientAsync(Guid id, Client client);
    }
}