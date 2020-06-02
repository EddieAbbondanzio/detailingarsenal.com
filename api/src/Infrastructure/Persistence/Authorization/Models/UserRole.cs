using System;

namespace DetailingArsenal.Infrastructure.Persistence.Models {
    public class UserRole {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}