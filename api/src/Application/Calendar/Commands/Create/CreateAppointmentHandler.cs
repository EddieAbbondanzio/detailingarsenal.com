using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;

namespace DetailingArsenal.Application {
    [Authorization(Action = "create", Scope = "appointments")]
    public class CreateAppointmentHandler : ActionHandler<CreateAppointmentCommand, AppointmentDto> {
        private IAppointmentRepo appointmentRepo;
        private IClientRepo clientRepo;
        private IMapper mapper;

        public CreateAppointmentHandler(IAppointmentRepo appointmentRepo, IClientRepo clientRepo, IMapper mapper) {
            this.appointmentRepo = appointmentRepo;
            this.clientRepo = clientRepo;
            this.mapper = mapper;
        }

        public async override Task<AppointmentDto> Execute(CreateAppointmentCommand command, User? user) {
            var client = await clientRepo.FindById(command.ClientId) ?? throw new EntityNotFoundException();

            var appointment = Appointment.Create(
                user!.Id,
                command.ServiceId,
                client.Id,
                command.Price,
                command.Notes
            );

            appointment.Blocks = command.Blocks.Select(t => AppointmentBlock.Create(
                appointment.Id,
                t.Start,
                t.End
            )).ToList();

            await appointmentRepo.Add(appointment);

            return mapper.Map<Appointment, AppointmentDto>(appointment);
        }
    }
}