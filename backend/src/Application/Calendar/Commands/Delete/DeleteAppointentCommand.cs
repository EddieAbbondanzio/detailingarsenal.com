using System;

namespace DetailingArsenal.Application {
    public class DeleteAppointmentCommand : IAction {
        public Guid Id { get; set; }
    }
}