using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api {
    [Authorize]
    [ApiController]
    [Route("settings/hours-of-operation")]
    public class HoursOfOperationController : ControllerBase {

        private IMediator mediator;

        public HoursOfOperationController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetHoursOfOperation() {
            HoursOfOperationDto? hours = await mediator.Dispatch<GetHoursOfOperationQuery, HoursOfOperationDto>(new GetHoursOfOperationQuery(), User.GetUserId());
            return Ok(hours);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHoursOfOperation(string id, [FromBody]UpdateHoursOfOperationCommand update) {
            HoursOfOperationDto hours = await mediator.Dispatch<UpdateHoursOfOperationCommand, HoursOfOperationDto>(update, User.GetUserId());
            return Ok(hours);
        }
    }
}