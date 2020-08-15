using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Clients {

    [Authorize]
    [Route("client")]
    [ApiController]
    public class ClientController : ControllerBase {
        private IMediator mediator;

        public ClientController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients() {
            var clients = await mediator.Dispatch<GetClientsQuery, List<ClientView>>(new GetClientsQuery(), User.GetUserId());
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientCommand command) {
            var client = await mediator.Dispatch<CreateClientCommand, ClientView>(command, User.GetUserId());
            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientCommand command) {
            var client = await mediator.Dispatch<UpdateClientCommand, ClientView>(command, User.GetUserId());
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id) {
            await mediator.Dispatch<DeleteClientCommand, ClientView>(new DeleteClientCommand() { Id = id }, User.GetUserId());
            return Ok();
        }
    }
}