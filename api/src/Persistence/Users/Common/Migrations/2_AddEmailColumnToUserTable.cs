using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Migrations {
    [Migration(2020_03_26_00, "Add email column to user table")]
    public class AddEmailColumnToUserTable : Migration {
        public override void Up() {
            Alter.Table("users").AddColumn("email").AsString(64 + 256).Nullable();
        }

        public override void Down() {
            Delete.Column("email").FromTable("users");
        }
    }
}