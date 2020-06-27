using System;

namespace DetailingArsenal.Persistence.Security {
    public class RolePermission {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
