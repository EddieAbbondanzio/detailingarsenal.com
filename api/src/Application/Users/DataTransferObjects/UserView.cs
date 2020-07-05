using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Users {
    public class UserView : IDataTransferObject {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
    }
}