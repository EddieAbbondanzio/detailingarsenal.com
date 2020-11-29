using System;

namespace DetailingArsenal.Persistence.Users {
    public class UserRow : IDataTransferObject {
        public Guid Id { get; set; }
        public string Auth0Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime JoinedDate { get; set; }
        public string? Name { get; set; }
    }
}