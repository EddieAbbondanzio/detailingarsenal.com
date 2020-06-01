using System;
using Dapper;
using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence {
    [Migration(2020_05_31_1, "Create admin role")]
    public class InsertAdminRole : Migration {
        public override void Up() {
            Execute.WithConnection((c, t) => {
                var roleId = Guid.NewGuid();

                c.Execute(
                    "insert into roles (id, name) values (@Id, @Name);",
                    new {
                        Id = roleId,
                        Name = "Admin"
                    }
                );

                var perm = c.QueryFirst<Permission>("select * from permissions where action = 'read' and scope = 'admin-panel';");

                c.Execute(
                    "insert into role_permissions (role_id, permission_id) values (@RoleId, @PermissionId);",
                    new {
                        RoleId = roleId,
                        PermissionId = perm.Id
                    }
                );
            });
        }

        public override void Down() {
            Execute.WithConnection((c, t) => {
                var role = c.QueryFirst<Role>("select * from roles where name = 'Admin';");

                c.Execute("delete from role_permissions where role_id = @Id;", role);
                c.Execute("delete from roles where id = @Id", role);
            });
        }
    }
}