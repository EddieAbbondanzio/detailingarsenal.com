using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class CreateEmployeeCommand : IAction {
        public string Name { get; set; } = null!;
        public string? Position { get; set; }
    }
}