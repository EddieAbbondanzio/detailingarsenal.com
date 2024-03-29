using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Calendar {
    [Authorization(Action = "update", Scope = "appointments")]
    [DependencyInjection]
    public class UpdateAppointmentHandler : ActionHandler<UpdateAppointmentCommand, AppointmentView> {
        IAppointmentService service;

        public UpdateAppointmentHandler(IAppointmentService service) {
            this.service = service;
        }

        public async override Task<AppointmentView> Execute(UpdateAppointmentCommand input, User? user) {
            var appointment = await service.GetById(input.Id);

            if (!appointment.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            var update = new AppointmentUpdate(
                input.ServiceId,
                input.ClientId,
                input.Price,
                input.Notes
            );

            update.Blocks = input.Blocks.Select(b => new AppointmentBlockUpdate(
                b.Start,
                b.Start
            )).ToList();

            await service.Update(appointment, update);
            throw new NotImplementedException();
            // return mapper.Map<Appointment, AppointmentView>(appointment);
        }
    }
}