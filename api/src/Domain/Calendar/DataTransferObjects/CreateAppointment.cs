using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Calendar {
    public class CreateAppointment : IDataTransferObject {
        public Guid ServiceId { get; }
        public Guid ClientId { get; }
        public decimal Price { get; }
        public string? Notes { get; }
        public List<CreateAppointmentBlock> Blocks { get; set; } = new List<CreateAppointmentBlock>();

        public CreateAppointment(Guid serviceId, Guid clientId, decimal price, string? notes) {
            ServiceId = serviceId;
            ClientId = clientId;
            Price = price;
            Notes = notes;
        }
    }
}