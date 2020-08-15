using System;

namespace DetailingArsenal.Domain.Settings {
    public class VehicleCategoryCreate : IDataTransferObject {
        public string Name { get; }
        public string? Description { get; }
        public VehicleCategoryCreate(string name, string? description = null) {
            Name = name;
            Description = description;
        }
    }
}