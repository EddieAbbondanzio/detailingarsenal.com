using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Calendar {
    [Authorization(Action = "create", Scope = "appointments")]
    [DependencyInjection]
    public class CreateAppointmentHandler : ActionHandler<CreateAppointmentCommand, AppointmentView> {
        IAppointmentService service;

        public CreateAppointmentHandler(IAppointmentService service) {
            this.service = service;
        }

        public async override Task<AppointmentView> Execute(CreateAppointmentCommand command, User? user) {
            var create = new AppointmentCreate(
                command.ServiceId,
                command.ClientId,
                command.Price,
                command.Notes
            );

            create.Blocks = command.Blocks.Select(b => new AppointmentBlockCreate(
                b.Start,
                b.End
            )).ToList();

            var appointment = await service.Create(create, user!);
            throw new NotImplementedException();
            // return mapper.Map<Appointment, AppointmentView>(appointment);
        }
    }
}