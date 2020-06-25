using System;

namespace DetailingArsenal.Infrastructure.Persistence.Models {
    public class RolePermission {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
