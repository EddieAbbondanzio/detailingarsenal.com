using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class UpdateClientCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}