using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Calendar {
    public class UpdateAppointment : IDataTransferObject {
        public Guid ServiceId { get; }
        public Guid ClientId { get; }
        public decimal Price { get; }
        public string? Notes { get; }
        public List<UpdateAppointmentBlock> Blocks { get; set; } = new List<UpdateAppointmentBlock>();

        public UpdateAppointment(Guid serviceId, Guid clientId, decimal price, string? notes) {
            ServiceId = serviceId;
            ClientId = clientId;
            Price = price;
            Notes = notes;
        }
    }
}