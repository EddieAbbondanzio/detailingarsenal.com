using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DetailingArsenal.Application.Settings;

namespace DetailingArsenal.Api.Settings {
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
            HoursOfOperationView? hours = await mediator.Dispatch<GetHoursOfOperationQuery, HoursOfOperationView>(new GetHoursOfOperationQuery(), User.GetUserId());
            return Ok(hours);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHoursOfOperation(string id, [FromBody] UpdateHoursOfOperationCommand update) {
            HoursOfOperationView hours = await mediator.Dispatch<UpdateHoursOfOperationCommand, HoursOfOperationView>(update, User.GetUserId());
            return Ok(hours);
        }
    }
}