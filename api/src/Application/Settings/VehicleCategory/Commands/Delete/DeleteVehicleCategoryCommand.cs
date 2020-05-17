using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class DeleteVehicleCategoryCommand : IAction {
        public Guid Id { get; set; } = Guid.Empty;
    }
}