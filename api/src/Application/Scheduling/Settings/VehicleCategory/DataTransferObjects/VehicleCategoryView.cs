using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class VehicleCategoryView : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}