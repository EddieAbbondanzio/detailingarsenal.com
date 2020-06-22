using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application {
    [Authorization(Action = "update", Scope = "appointments")]
    public class UpdateAppointmentHandler : ActionHandler<UpdateAppointmentCommand, AppointmentDto> {
        private IClientRepo clientRepo;
        private IAppointmentRepo appointmentRepo;
        private IMapper mapper;

        public UpdateAppointmentHandler(IAppointmentRepo appointmentRepo, IClientRepo clientRepo, IMapper mapper) {
            this.appointmentRepo = appointmentRepo;
            this.clientRepo = clientRepo;
            this.mapper = mapper;
        }

        public async override Task<AppointmentDto> Execute(UpdateAppointmentCommand input, User? user) {
            var client = await clientRepo.FindById(input.ClientId) ?? throw new EntityNotFoundException();

            var appointment = await appointmentRepo.FindById(input.Id) ?? throw new EntityNotFoundException();

            if (!appointment.IsOwner(user!)) {
                throw new AuthorizationException();
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