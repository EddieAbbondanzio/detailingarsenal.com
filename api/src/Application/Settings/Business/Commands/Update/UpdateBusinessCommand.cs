using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class UpdateBusinessCommand : IAction {
        public Guid Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}