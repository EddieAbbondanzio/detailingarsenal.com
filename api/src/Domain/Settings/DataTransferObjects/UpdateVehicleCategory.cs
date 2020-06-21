using System;

namespace DetailingArsenal.Domain.Settings {
    public class UpdateVehicleCategory : IDataTransferObject {
        public string Name { get; }
        public string? Description { get; }

        public UpdateVehicleCategory(string name, string? description = null) {
            Name = name;
            Description = description;
        }
    }
}