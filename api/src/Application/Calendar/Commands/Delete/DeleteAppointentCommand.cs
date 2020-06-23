using System;

namespace DetailingArsenal.Application.Calendar {
    public class DeleteAppointmentCommand : IAction {
        public Guid Id { get; set; }
    }
}