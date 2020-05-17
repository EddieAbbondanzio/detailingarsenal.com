using System;

namespace DetailingArsenal.Application {
    public class VehicleCategoryDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}