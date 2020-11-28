using System;
using System.Collections.Generic;

namespace DetailingArsenal.Api.Users.Security {
    public class RoleUpdateRequest : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Guid> PermissionIds { get; set; } = null!;
    }
}