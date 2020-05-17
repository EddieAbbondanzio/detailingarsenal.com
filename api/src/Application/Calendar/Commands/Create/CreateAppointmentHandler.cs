using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class CreateAppointmentHandler : ActionHandler<CreateAppointmentCommand, AppointmentDto> {
        private IAppointmentRepo appointmentRepo;
        private IClientRepo clientRepo;
        private IMapper mapper;

        public CreateAppointmentHandler(IAppointmentRepo appointmentRepo, IClientRepo clientRepo, IMapper mapper) {
            this.appointmentRepo = appointmentRepo;
            this.clientRepo = clientRepo;
            this.mapper = mapper;
        }

        protected async override Task<AppointmentDto> Execute(CreateAppointmentCommand command, User? user) {
            var client = await clientRepo.FindById(command.ClientId) ?? throw new EntityNotFoundException();

            var appointment = new Appointment() {
                Id = Guid.NewGuid(),
                UserId = user!.Id,
                ServiceId = command.ServiceId,
                ClientId = client.Id,
                Price = command.Price,
                Notes = command.Notes
            };

            appointment.Blocks = command.Blocks.Select(t => new AppointmentBlock() {
                Id = Guid.NewGuid(),
                AppointmentId = appointment.Id,
                Start = t.Start,
                End = t.End
            }).ToList();

            await appointmentRepo.Add(appointment);

            return mapper.Map<Appointment, AppointmentDto>(appointment);
        }
    }
}