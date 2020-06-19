using System;

namespace DetailingArsenal.Domain.Settings {
    public class UpdateVehicleCategory : IDataTransferObject {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}