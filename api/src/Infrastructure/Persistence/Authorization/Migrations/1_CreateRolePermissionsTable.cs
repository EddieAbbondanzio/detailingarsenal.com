using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_05_29_0, "Create role permissions table")]
    public class CreateRolePermissionsTable : Migration {
        public override void Up() {
            Create.Table("role_permissions")
                .WithColumn("role_id").AsGuid().ForeignKey("roles", "id")
                .WithColumn("permission_id").AsGuid().ForeignKey("permissions", "id");
        }

        public override void Down() {
            Delete.Table("role_permissions");
        }
    }
}