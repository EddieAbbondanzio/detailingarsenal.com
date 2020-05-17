using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application {
    public class CreateAppointmentCommand : IAction {
        public Guid ServiceId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public List<AppointmentBlockDto> Blocks { get; set; } = new List<AppointmentBlockDto>();
        public string? Notes { get; set; }
    }
}