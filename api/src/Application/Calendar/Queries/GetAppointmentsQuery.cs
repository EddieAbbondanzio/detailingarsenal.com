using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;

namespace DetailingArsenal.Application.Calendar {
    public class GetAppointmentsQuery : IAction {
        public AppointmentRange Range { get; set; }
        public DateTime Date { get; set; }
    }
}