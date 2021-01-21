using System;
using System.Linq;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Security.Migrations {
    [Migration(2020_08_24_02, "Insert crud pad series permissions")]
    public class InsertCrudPadSeriesPermissions : Migration {
        const string Scope = "pad-series";

        public override void Up() {
            Execute.WithConnection((c, t) => {
                var permissions = new dynamic[]{
                    new {
                        Id = Guid.NewGuid(),
                        Action = "create",
                        Scope = Scope
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "read",
                        Scope = Scope
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "update",
                        Scope = Scope
                    },
                    new {
                        Id = Guid.NewGuid(),
                        Action = "delete",
                        Scope = Scope
                    }
                };

                c.Execute(
                    "insert into permissions (id, action, scope) values (@Id, @Action, @Scope);",
                    permissions
                );

                // Add new permission to admin
                var adminRole = c.QueryFirst<RoleRow>("select * from roles where name = 'Admin';");
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
                var permission = c.QueryFirst<PermissionRow>(
                    $@"select * from permissions where scope = '{Scope}' 
                    and (action = 'create' or action = 'read' or action = 'update' or action = 'delete');"
                );

                c.Execute("delete from role_permissions where permission_id = @Id", permission);
                c.Execute("delete from permissions where id = @Id", permission);
            });
        }
    }
}