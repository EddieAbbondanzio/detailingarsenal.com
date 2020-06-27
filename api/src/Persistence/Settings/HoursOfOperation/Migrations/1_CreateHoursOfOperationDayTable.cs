using FluentMigrator;

namespace DetailingArsenal.Persistance.Settings.Migrations {
    [Migration(2020_03_20_1, "Create hours_of_operation_days table")]
    public class CreateHoursOfOperationDayTable : Migration {
        public override void Up() {
            Create.Table("hours_of_operation_days")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("hours_of_operation_id").AsGuid().ForeignKey("hours_of_operations", "id")
                .WithColumn("day").AsInt16()
                .WithColumn("open").AsInt32()
                .WithColumn("close").AsInt32()
                .WithColumn("enabled").AsBoolean();
        }

        public override void Down() {
            Delete.Table("hours_of_operation_days");
        }
    }
}