using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Calendar {
    public class AppointmentDto : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; } = null!;
        public List<AppointmentBlockDto> Blocks { get; set; } = null!;
    }
}