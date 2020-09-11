using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Migrations {
    [Migration(2020_09_10, "Add username column to users")]
    public class AddUsernameColumnToUsers : Migration {
        public override void Up() {
            Alter.Table("users").AddColumn("username").AsString(32);
        }

        public override void Down() {
            Delete.Column("username").FromTable("users");
        }
    }
}