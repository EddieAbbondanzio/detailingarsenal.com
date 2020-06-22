using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Security {
    public class RoleDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}