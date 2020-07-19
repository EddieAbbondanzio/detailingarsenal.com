using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Security {
    public class RoleCreate : IDataTransferObject {
        public string Name { get; }
        public List<Guid> PermissionIds { get; }

        public RoleCreate(string name, List<Guid> permissionIds) {
            Name = name;
            PermissionIds = permissionIds;
        }
    }
}