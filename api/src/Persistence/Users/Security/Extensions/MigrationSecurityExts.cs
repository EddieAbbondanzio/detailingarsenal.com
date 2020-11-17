using System;
using System.Linq;
using Dapper;
using DetailingArsenal.Domain.Users.Security;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Security {
    public static class MigrationSecurityExts {
        public static void AddCrudPermissions(this Migration migration, string scope) {
            migration.Execute.WithConnection((c, t) => {
                var permissions = new dynamic[]{
                    new {
                        Id = Guid.NewGuid(),
                        Action = "create",
                        Scope = scope
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "read",
                        Scope = scope
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "update",
                        Scope = scope
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "delete",
                        Scope = scope
                    }
                };

                c.Execute(
                    "insert into permissions (id, action, scope) values (@Id, @Action, @Scope);",
                    permissions
                );

                // Add new permission to admin
                var adminRole = c.QueryFirst<Role>("select * from roles where name = 'Admin';");
                c.Execute(
                    "insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);",
                    permissions.Select(p => new {
                        RoleId = adminRole.Id,
                        PermissionId = p.Id
                    }).ToList()
                );
            });
        }

        public static void RemoveCrudPermissions(this Migration migration, string scope) {
            migration.Execute.WithConnection((c, t) => {
                var permission = c.QueryFirst<Permission>(
                    $@"select * from permissions where scope = '{scope}' 
                    and (action = 'create' or action = 'read' or action = 'update' or action = 'delete');"
                );

                c.Execute("delete from role_permissions where permission_id = @Id", permission);
                c.Execute("delete from permissions where id = @Id", permission);
            });
        }
    }
}