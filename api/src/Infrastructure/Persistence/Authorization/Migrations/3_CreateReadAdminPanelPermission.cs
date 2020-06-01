using System;
using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence {
    [Migration(2020_05_31_0, "Create read admin panel permission")]
    public class CreateReadAdminPanelPermission : Migration {
        public override void Up() {
            Insert.IntoTable("permissions").Row(new {
                id = Guid.NewGuid(),
                action = "read",
                scope = "admin-panel"
            });
        }

        public override void Down() {
            Delete.FromTable("permissions").Row(new {
                action = "read",
                scope = "admin-panel"
            });
        }
    }
}