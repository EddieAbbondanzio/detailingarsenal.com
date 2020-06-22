using System;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence {
    [Migration(2020_06_01_0, "Insert add role to users permission")]
    public class InsertAddRolePermission : Migration {
        public override void Up() {
            Execute.WithConnection((c, t) => {
                var permission = new {
                    Id = Guid.NewGuid(),
                    Action = "add-role",
                    Scope = "users"
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
                var permission = c.QueryFirst<Permission>("select * from permissions where action = 'add-role' and scope = 'users';");

                c.Execute("delete from role_permissions where permission_id = @Id", permission);
                c.Execute("delete from permissions where id = @Id", permission);
            });
        }
    }
}