using System;

namespace DetailingArsenal.Persistence.Security {
    public class UserRole {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}