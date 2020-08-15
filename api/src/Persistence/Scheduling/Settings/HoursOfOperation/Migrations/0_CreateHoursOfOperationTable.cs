using FluentMigrator;

namespace DetailingArsenal.Persistence.Settings.Migrations {
    [Migration(2020_03_20_0, "Create hours_of_operations table")]
    public class CreateHoursOfOperationTable : Migration {
        public override void Up() {
            Create.Table("hours_of_operations")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id");
        }

        public override void Down() {
            Delete.Table("hours_of_operations");
        }
    }
}