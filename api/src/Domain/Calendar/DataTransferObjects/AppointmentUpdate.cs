using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Calendar {
    public class AppointmentUpdate : IDataTransferObject {
        public Guid ServiceId { get; }
        public Guid ClientId { get; }
        public decimal Price { get; }
        public string? Notes { get; }
        public List<AppointmentBlockUpdate> Blocks { get; set; } = new List<AppointmentBlockUpdate>();

        public AppointmentUpdate(Guid serviceId, Guid clientId, decimal price, string? notes) {
            ServiceId = serviceId;
            ClientId = clientId;
            Price = price;
            Notes = notes;
        }
    }
}