using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Calendar {
    [Authorization(Action = "create", Scope = "appointments")]
    public class CreateAppointmentHandler : ActionHandler<CreateAppointmentCommand, AppointmentView> {
        IAppointmentService service;
        private IMapper mapper;

        public CreateAppointmentHandler(IAppointmentService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<AppointmentView> Execute(CreateAppointmentCommand command, User? user) {
            var create = new CreateAppointment(
                command.ServiceId,
                command.ClientId,
                command.Price,
                command.Notes
            );

            create.Blocks = command.Blocks.Select(b => new CreateAppointmentBlock(
                b.Start,
                b.End
            )).ToList();

            var appointment = await service.Create(create, user!);
            return mapper.Map<Appointment, AppointmentView>(appointment);
        }
    }
}