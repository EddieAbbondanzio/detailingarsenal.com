using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class GetAppointmentsQuery : IAction {
        public AppointmentRange Range { get; set; }
        public DateTime Date { get; set; }
    }
}