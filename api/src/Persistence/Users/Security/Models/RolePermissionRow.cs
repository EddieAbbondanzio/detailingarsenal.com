using System;

namespace DetailingArsenal.Persistence.Users.Security {
    internal class RolePermissionRow : IDataTransferObject {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
