using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Calendar {
    [Authorization(Action = "read", Scope = "appointments")]
    public class GetAppointmentsHandler : ActionHandler<GetAppointmentsQuery, List<AppointmentView>> {
        IAppointmentService service;
        private IMapper mapper;

        public GetAppointmentsHandler(IAppointmentService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<AppointmentView>> Execute(GetAppointmentsQuery input, User? user) {
            List<Appointment> appointments = await service.GetWithinRange(input.Date, input.Range, user!);
            return mapper.Map<List<Appointment>, List<AppointmentView>>(appointments);
        }
    }
}