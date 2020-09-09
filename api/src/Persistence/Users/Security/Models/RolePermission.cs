using System;

namespace DetailingArsenal.Persistence.Users.Security {
    public class RolePermission {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
