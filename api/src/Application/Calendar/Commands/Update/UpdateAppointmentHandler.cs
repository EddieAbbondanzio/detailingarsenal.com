using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class UpdateAppointmentHandler : ActionHandler<UpdateAppointmentCommand, AppointmentDto> {
        private IClientRepo clientRepo;
        private IAppointmentRepo appointmentRepo;
        private IMapper mapper;

        public UpdateAppointmentHandler(IAppointmentRepo appointmentRepo, IClientRepo clientRepo, IMapper mapper) {
            this.appointmentRepo = appointmentRepo;
            this.clientRepo = clientRepo;
            this.mapper = mapper;
        }

        protected async override Task<AppointmentDto> Execute(UpdateAppointmentCommand input, User? user) {
            var client = await clientRepo.FindById(input.ClientId) ?? throw new EntityNotFoundException();

            var appointment = await appointmentRepo.FindById(input.Id) ?? throw new EntityNotFoundException();

            if (appointment.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            appointment.ServiceId = appointment.ServiceId;
            appointment.ClientId = client.Id;
            appointment.Price = appointment.Price;
            appointment.Notes = appointment.Notes;

            appointment.Blocks = input.Blocks.Select(t => new AppointmentBlock() {
                Id = Guid.NewGuid(),
                AppointmentId = appointment.Id,
                Start = t.Start,
                End = t.End
            }).ToList();

            await appointmentRepo.Update(appointment);
            return mapper.Map<Appointment, AppointmentDto>(appointment);
        }
    }
}