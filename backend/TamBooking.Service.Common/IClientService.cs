using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IClientService
    {
        public Task CreateClientAsync(Client client);

        public Task<Client> GetClientAsync(Guid id);

        public Task<bool> UpdateClientAsync(Guid id, Client client);
    }
}