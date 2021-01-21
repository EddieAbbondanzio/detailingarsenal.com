using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Migrations {
    // Needs to be ran first since other tables reference it.
    [Migration(0000_00_00_00, "Create users table")]
    public class CreateUserTable : Migration {
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