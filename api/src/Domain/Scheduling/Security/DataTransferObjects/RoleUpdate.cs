using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Security {
    public class RoleUpdate : IDataTransferObject {
        public string Name { get; }
        public List<Guid> PermissionIds { get; }

        public RoleUpdate(string name, List<Guid> permissionIds) {
            Name = name;
            PermissionIds = permissionIds;
        }
    }
}