using System;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence {
    [Migration(2020_05_31_0, "Create admin role")]
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
            });
        }

        public override void Down() {
            Execute.WithConnection((c, t) => {
                var role = c.QueryFirst<Role>("select * from roles where name = 'Admin';");

                c.Execute("delete from roles where id = @Id", role);
            });
        }
    }
}