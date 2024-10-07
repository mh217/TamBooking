using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TamBooking.Model;
using TamBooking.Service.Common;
using TamBooking.WebAPI.RESTModels;

namespace TamBooking.WebAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> CreateClientAsync(ClientInfo clientInfo)
        {
            Client client = new()
            {
                FirstName = clientInfo.FirstName,
                LastName = clientInfo.LastName,
                TownId = clientInfo.TownId
            };
            await _clientService.CreateClientAsync(client);
            return StatusCode(201);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetClientAsync(Guid id)
        {
            GetClient clientGet = new();
            Town town = new();
            County county = new();
            var client = await _clientService.GetClientAsync(id);
            if (client is null)
            {
                return NotFound("Client not found");
            }
            clientGet.FirstName = client.FirstName;
            clientGet.LastName = client.LastName;
            town.Id = client.TownId;
            town = client.Town;
            county.Name = town.County.Name;
            county.Id = town.CountyId;
            clientGet.County = county;
            clientGet.Town = town;
            return Ok(clientGet);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateClientAsync(Guid id, [FromBody] UpdateClient updateClient)
        {
            Client client = new Client();
            client.FirstName = updateClient.FirstName;
            client.LastName = updateClient.LastName;
            client.TownId = updateClient.TownId;
            var isClientUpdated = await _clientService.UpdateClientAsync(id, client);
            if (!isClientUpdated)
            {
                return NotFound("Client not found");
            }
            return NoContent();
        }
    }
}