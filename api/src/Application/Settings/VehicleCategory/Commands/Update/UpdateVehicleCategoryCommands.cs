using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class UpdateVehicleCategoryCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}