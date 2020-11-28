
using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Users.Security {
    public class Role : Aggregate<Role> {
        public const int NameMaxLength = 32;

        public string Name { get; set; } = null!;
        public List<Guid> PermissionIds { get; set; }

        public Role(Guid id, string name, List<Guid>? permissionIds = null) {
            Id = id;
            Name = name;
            PermissionIds = permissionIds ?? new List<Guid>();
        }

        public Role(string name, List<Guid>? permissionIds = null) {
            Id = Guid.NewGuid();
            Name = name;
            PermissionIds = permissionIds ?? new List<Guid>();
        }
    }
}