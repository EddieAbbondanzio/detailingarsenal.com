using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_05_28_0, "Create permissions table")]
    public class CreatePermissionsTable : Migration {
        public override void Up() {
            Create.Table("permissions")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("action").AsString(32)
                .WithColumn("scope").AsString(32);
        }

        public override void Down() {
            Delete.Table("permissions");
        }
    }
}