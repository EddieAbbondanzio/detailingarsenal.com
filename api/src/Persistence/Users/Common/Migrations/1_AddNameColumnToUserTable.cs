using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Migrations {
    [Migration(2020_03_25_00, "Add name column to user table")]
    public class AddNameColumnToUserTable : Migration {
        public override void Up() {
            Alter.Table("users").AddColumn("name").AsString(64).Nullable();
        }

        public override void Down() {
            Delete.Column("name").FromTable("users");
        }
    }
}