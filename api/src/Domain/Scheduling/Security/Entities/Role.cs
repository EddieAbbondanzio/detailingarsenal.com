
using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Security {
    public class Role : Aggregate<Role> {
        public const int NameMaxLength = 32;

        public string Name { get; set; } = null!;
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();

        public static Role Create(string name, List<Guid> permissionIds) {
            return new Role() {
                Id = Guid.NewGuid(),
                Name = name,
                PermissionIds = permissionIds
            };
        }
    }
}