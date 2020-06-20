using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class DeleteVehicleCategoryCommand : IAction {
        public Guid Id { get; set; } = Guid.Empty;
    }
}