using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class CreateClientCommand : IAction {
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}