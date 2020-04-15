using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class UpdateEmployeeCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Position { get; set; }
    }
}