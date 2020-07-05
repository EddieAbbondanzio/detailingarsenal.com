using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Clients {
    public class ClientView : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}