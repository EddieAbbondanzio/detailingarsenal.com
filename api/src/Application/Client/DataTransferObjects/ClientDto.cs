using System;

namespace DetailingArsenal.Application {
    public class ClientDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}