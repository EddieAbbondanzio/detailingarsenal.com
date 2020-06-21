using System;

namespace DetailingArsenal.Domain.Settings {
    public class CreateVehicleCategory : IDataTransferObject {
        public string Name { get; }
        public string? Description { get; }
        public CreateVehicleCategory(string name, string? description = null) {
            Name = name;
            Description = description;
        }
    }
}