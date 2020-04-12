using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api {
    [Authorize]
    [ApiController]
    [Route("settings/service")]
    public class ServiceController : ControllerBase {
        private IMediator mediator;

        public ServiceController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetServices() {
            List<ServiceDto> services = await mediator.Dispatch<GetServicesQuery, List<ServiceDto>>(
                new GetServicesQuery(),
                User.GetUserId()
            );

            return Ok(services);
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(CreateServiceCommand command) {
            ServiceDto s = await mediator.Dispatch<CreateServiceCommand, ServiceDto>(
                command,
                User.GetUserId()
            );

            return Ok(s);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody]UpdateServiceCommand command) {
            ServiceDto s = await mediator.Dispatch<UpdateServiceCommand, ServiceDto>(
                command,
                User.GetUserId()
            );

            return Ok(s);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id) {
            await mediator.Dispatch<DeleteServiceCommand>(
                new DeleteServiceCommand() { Id = id },
                User.GetUserId()
            );

            return Ok();
        }
    }
}