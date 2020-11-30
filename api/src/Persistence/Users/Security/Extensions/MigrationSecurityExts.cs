using System;
using System.Linq;
using Dapper;
using DetailingArsenal.Domain.Users.Security;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Security {
    public static class MigrationSecurityExts {
        public static void AddPermission(this Migration migration, string scope, string action) {
            migration.Execute.WithConnection((c, t) => {
                var perm = new {
                    Id = Guid.NewGuid(),
                    Action = action,
                    Scope = scope
                };

                c.Execute(
                    "insert into permissions (id, action, scope) values (@Id, @Action, @Scope);", perm);
            });
        }
        public static void RemovePermissions(this Migration migration, string scope, string action) {
            migration.Execute.WithConnection((c, t) => {
                var permission = c.QueryFirst<PermissionRow>(
                    $@"select * from permissions where scope = '{scope}' 
                    and action = '{action}';"
                );

                c.Execute("delete from role_permissions where permission_id = @Id", permission);
                c.Execute("delete from permissions where id = @Id", permission);
            });
        }

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
            });
        }

        public static void RemoveCrudPermissions(this Migration migration, string scope) {
            // Does this even work?
            migration.Execute.WithConnection((c, t) => {
                var permission = c.QueryFirst<PermissionRow>(
                    $@"select * from permissions where scope = '{scope}' 
                    and (action = 'create' or action = 'read' or action = 'update' or action = 'delete');"
                );

                c.Execute("delete from role_permissions where permission_id = @Id", permission);
                c.Execute("delete from permissions where id = @Id", permission);
            });
        }
    }
}