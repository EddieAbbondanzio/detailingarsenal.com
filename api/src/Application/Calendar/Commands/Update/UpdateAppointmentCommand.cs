using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application {
    public class UpdateAppointmentCommand : IAction {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public List<AppointmentBlockDto> Blocks { get; set; } = new List<AppointmentBlockDto>();
        public string? Notes { get; set; }
    }
}