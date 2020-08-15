using System;

namespace DetailingArsenal.Domain.Settings {
    public class VehicleCategoryUpdate : IDataTransferObject {
        public string Name { get; }
        public string? Description { get; }

        public VehicleCategoryUpdate(string name, string? description = null) {
            Name = name;
            Description = description;
        }
    }
}