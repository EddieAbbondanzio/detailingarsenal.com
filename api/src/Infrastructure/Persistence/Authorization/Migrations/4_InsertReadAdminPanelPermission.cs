using System;
using Dapper;
using DetailingArsenal.Domain;
using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence {
    [Migration(2020_05_31_1, "Create read admin panel permission")]
    public class InsertReadAdminPanelPermission : Migration {
        public override void Up() {
            Execute.WithConnection((c, t) => {
                var permission = new {
                    Id = Guid.NewGuid(),
                    Action = "read",
                    Scope = "admin-panel"
                };

                c.Execute(
                    "insert into permissions (id, action, scope) values (@Id, @Action, @Scope);",
                    permission
                );

                // Add new permission to admin
                var adminRole = c.QueryFirst<Role>("select * from roles where name = 'Admin';");
                c.Execute(
                    "insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);",
                    new {
                        RoleId = adminRole.Id,
                        PermissionId = permission.Id
                    }
                );
            });
        }

        public override void Down() {
            Execute.WithConnection((c, t) => {
                var permission = c.QueryFirst<Permission>("select * from permissions where action = 'read' and scope = 'admin-panel';");

                c.Execute("delete from role_permissions where permission_id = @Id", permission);
                c.Execute("delete from permissions where id = @Id", permission);
            });
        }
    }
}