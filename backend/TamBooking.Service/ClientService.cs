using Microsoft.AspNetCore.Http;
using TamBooking.Model;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientService(IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
        {
            _clientRepository = clientRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateClientAsync(Client client)
        {
            var currentUserId = _jwtService.GetCurrentUserClaims().Id;
            client.Id = Guid.Parse(currentUserId);
            await _clientRepository.CreateClientAsync(client);
        }

        public async Task<Client?> GetClientAsync(Guid id)
        {
            var client = await _clientRepository.GetClientAsync(id);
            return client;
        }

        public async Task<bool> UpdateClientAsync(Guid id, Client client)
        {
            return await _clientRepository.UpdateClientAsync(id, client);
        }
    }
}