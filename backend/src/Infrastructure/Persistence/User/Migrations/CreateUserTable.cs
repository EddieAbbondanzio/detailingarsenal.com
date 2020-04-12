using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    // Needs to be ran first since other tables reference it.
    [Migration(2020_00_00_0, "Create users table")]
    public class CreateUserTableMigration : Migration {
        public override void Up() {
            Create.Table("users")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("auth_0_id").AsString(32).Unique();
        }

        public override void Down() {
            Delete.Table("users");
        }
    }
}