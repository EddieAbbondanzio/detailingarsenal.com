using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Migrations {
    [Migration(2020_07_21_0, "Add joined_date column to users")]
    public class AddJoinedDateColumnToUsers : Migration {
        public override void Up() {
            Alter.Table("users").AddColumn("joined_date").AsDateTime();
        }

        public override void Down() {
            Delete.Column("joined_date").FromTable("users");
        }
    }
}