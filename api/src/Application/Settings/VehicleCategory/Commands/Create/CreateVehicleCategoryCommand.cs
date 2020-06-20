using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class CreateVehicleCategoryCommand : IAction {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}