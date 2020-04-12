using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_03_25_0, "Add name column to user table")]
    public class AddNameColumnToUserTable : Migration {
        public override void Up() {
            Alter.Table("users").AddColumn("name").AsString(64).Nullable();
        }

        public override void Down() {
            Delete.Column("name").FromTable("users");
        }
    }
}