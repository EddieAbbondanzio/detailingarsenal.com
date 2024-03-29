using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Calendar;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Calendar {
    [Authorize]
    [ApiController]
    [Route("appointment")]
    public class AppointmentController : ControllerBase {
        private IMediator mediator;

        public AppointmentController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string range, [FromQuery] DateTime date) {
            AppointmentRange appointmentRange;

            if (!Enum.TryParse(range, true, out appointmentRange)) {
                return BadRequest("Invalid range specified");
            }

            var appointments = await mediator.Dispatch<GetAppointmentsQuery, List<AppointmentView>>(
                new GetAppointmentsQuery() { Range = appointmentRange, Date = date },
                User.GetUserId()
            );

            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentCommand command) {
            AppointmentView appointment = await mediator.Dispatch<CreateAppointmentCommand, AppointmentView>(command, User.GetUserId());
            return Ok(appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateAppointmentCommand command) {
            AppointmentView appointment = await mediator.Dispatch<UpdateAppointmentCommand, AppointmentView>(command, User.GetUserId());
            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Dispatch<DeleteAppointmentCommand>(new DeleteAppointmentCommand() {
                Id = id
            },
                User.GetUserId());

            return Ok();
        }
    }
}