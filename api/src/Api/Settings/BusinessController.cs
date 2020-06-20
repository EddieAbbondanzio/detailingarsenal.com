using System;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Settings;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Settings {
    [Authorize]
    [ApiController]
    [Route("settings/business")]
    public class BusinessController : ControllerBase {
        private IMediator mediator;

        public BusinessController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBusiness() {
            BusinessDto b = await mediator.Dispatch<GetBusinessQuery, BusinessDto>(new GetBusinessQuery(), User.GetUserId());
            return Ok(b);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusiness(Guid id, [FromBody] UpdateBusinessCommand update) {
            BusinessDto b = await mediator.Dispatch<UpdateBusinessCommand, BusinessDto>(update, User.GetUserId());
            return Ok(b);
        }
    }
}