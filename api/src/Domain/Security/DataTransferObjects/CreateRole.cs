using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Security {
    public class CreateRole : IDataTransferObject {
        public string Name { get; }
        public List<Guid> PermissionIds { get; }

        public CreateRole(string name, List<Guid> permissionIds) {
            Name = name;
            PermissionIds = permissionIds;
        }
    }
}