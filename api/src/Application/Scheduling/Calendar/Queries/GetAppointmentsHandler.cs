using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Calendar {
    [Authorization(Action = "read", Scope = "appointments")]
    [DependencyInjection]
    public class GetAppointmentsHandler : ActionHandler<GetAppointmentsQuery, List<AppointmentView>> {
        IAppointmentService service;

        public GetAppointmentsHandler(IAppointmentService service) {
            this.service = service;
        }

        public async override Task<List<AppointmentView>> Execute(GetAppointmentsQuery input, User? user) {
            List<Appointment> appointments = await service.GetWithinRange(input.Date, input.Range, user!);
            throw new NotImplementedException();
            // return mapper.Map<List<Appointment>, List<AppointmentView>>(appointments);
        }
    }
}