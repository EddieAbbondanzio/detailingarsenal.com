using System;
using System.Linq;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence {
    [Migration(2020_06_02_2, "Insert crud permission permissions")]
    public class InsertCrudPermissionPermissions : Migration {
        public override void Up() {
            Execute.WithConnection((c, t) => {
                var permissions = new dynamic[]{
                    new {
                        Id = Guid.NewGuid(),
                        Action = "create",
                        Scope = "permissions"
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "read",
                        Scope = "permissions"
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "update",
                        Scope = "permissions"
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "delete",
                        Scope = "permissions"
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

        public override void Down() {
            Execute.WithConnection((c, t) => {
                var permission = c.QueryFirst<Permission>(
                    @"select * from permissions where scope = 'permissions' 
                    and (action = 'create' or action = 'read' or action = 'update' or action = 'delete');"
                );

                c.Execute("delete from role_permissions where permission_id = @Id", permission);
                c.Execute("delete from permissions where id = @Id", permission);
            });
        }
    }
}