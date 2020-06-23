using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Calendar {
    [Authorization(Action = "update", Scope = "appointments")]
    public class UpdateAppointmentHandler : ActionHandler<UpdateAppointmentCommand, AppointmentDto> {
        IAppointmentService service;
        IMapper mapper;

        public UpdateAppointmentHandler(IAppointmentService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<AppointmentDto> Execute(UpdateAppointmentCommand input, User? user) {
            var appointment = await service.GetById(input.Id);

            if (!appointment.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            var update = new UpdateAppointment(
                input.ServiceId,
                input.ClientId,
                input.Price,
                input.Notes
            );

            update.Blocks = input.Blocks.Select(b => new UpdateAppointmentBlock(
                b.Start,
                b.Start
            )).ToList();

            await service.Update(appointment, update);
            return mapper.Map<Appointment, AppointmentDto>(appointment);
        }
    }
}