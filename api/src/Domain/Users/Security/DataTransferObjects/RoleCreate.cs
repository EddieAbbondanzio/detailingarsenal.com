using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Users.Security {
    public class RoleCreate : IDataTransferObject {
        public string Name { get; }
        public List<Guid> PermissionIds { get; }

        public RoleCreate(string name, List<Guid>? permissionIds = null) {
            Name = name;
            PermissionIds = permissionIds ?? new List<Guid>();
        }
    }
}