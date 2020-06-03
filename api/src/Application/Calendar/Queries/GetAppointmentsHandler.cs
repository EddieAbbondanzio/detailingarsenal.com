using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "read", Scope = "appointments")]
    public class GetAppointmentsHandler : ActionHandler<GetAppointmentsQuery, List<AppointmentDto>> {
        private IAppointmentRepo repo;
        private IMapper mapper;

        public GetAppointmentsHandler(IAppointmentRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<List<AppointmentDto>> Execute(GetAppointmentsQuery input, User? user) {
            List<Appointment> appointments;

            if (input.Range == AppointmentRange.Day) {
                appointments = await repo.FindForDay(input.Date, user!);
            } else {
                appointments = await repo.FindForWeek(input.Date, user!);
            }

            return mapper.Map<List<Appointment>, List<AppointmentDto>>(appointments);
        }
    }
}