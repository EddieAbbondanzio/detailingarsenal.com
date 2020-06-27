using FluentMigrator;

namespace DetailingArsenal.Persistence.Migrations.Security.Migrations {
    [Migration(2020_06_02_1, "Create user roles table")]
    public class CreateUserRolesTable : Migration {
        public override void Up() {
            Create.Table("user_roles")
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("role_id").AsGuid().ForeignKey("roles", "id");

            Create.UniqueConstraint().OnTable("user_roles").Columns("user_id", "role_id");
        }

        public override void Down() {
            Delete.Table("user_roles");
        }
    }
}