using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Security {
    public class UpdateRole : IDataTransferObject {
        public string Name { get; }
        public List<Guid> PermissionIds { get; }

        public UpdateRole(string name, List<Guid> permissionIds) {
            Name = name;
            PermissionIds = permissionIds;
        }
    }
}