using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class CreateVehicleCategoryCommand : IAction {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}