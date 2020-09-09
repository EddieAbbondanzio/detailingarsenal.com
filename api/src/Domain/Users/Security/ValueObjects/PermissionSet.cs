using System;
using System.Collections.Generic;
using System.Linq;

namespace DetailingArsenal.Domain.Users.Security {
    public class PermissionSet : ValueObject<PermissionSet> {
        IEnumerable<Permission> permissions;

        public PermissionSet(IEnumerable<Permission> permissions) {
            this.permissions = permissions;
        }

        public bool HasPermission(string action, string scope) {
            foreach (Permission p in permissions) {
                if (p.Action == action && p.Scope == scope) {
                    return true;
                }
            }

            return false;
        }

        public List<Permission> ToList() {
            return this.permissions.ToList();
        }
    }
}