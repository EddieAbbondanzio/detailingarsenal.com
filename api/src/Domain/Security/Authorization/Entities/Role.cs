
using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain {
    public class Role : Aggregate<Role> {
        public const int NameMaxLength = 32;

        public string Name { get; set; } = null!;
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}