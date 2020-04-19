using FluentMigrator;

namespace DetailingArsenal.Persistance {
    [Migration(2020_04_18_1, "Delete vehicle_category_id column on appointment table")]
    public class DeleteDurationColumnOnAppointmentTable : Migration {
        public override void Up() {
            Delete.Column("vehicle_category_id").FromTable("appointments");
        }

        public override void Down() {
            Alter.Table("appointments").AddColumn("vehicle_category_id").AsGuid().ForeignKey("vehicle_categories", "id");
        }
    }
}