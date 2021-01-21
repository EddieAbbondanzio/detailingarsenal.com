using FluentMigrator;

namespace DetailingArsenal.Persistence.Calendar.Migrations {
    [Migration(2020_03_31_00, "Delete duration column on appointment table")]
    public class DeleteDurationColumnOnAppointmentTable : Migration {
        public override void Up() {
            Delete.Column("duration").FromTable("appointments");
        }

        public override void Down() {
            Alter.Table("appointments").AddColumn("duration").AsInt32();
        }
    }
}