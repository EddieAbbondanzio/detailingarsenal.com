using FluentMigrator;

namespace DetailingArsenal.Persistence.Calendar.Migrations {
    [Migration(2020_03_29_1, "Create appointment_times table")]
    public class CreateAppointmentTimesTable : Migration {
        public override void Up() {
            Create.Table("appointment_times")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("appointment_id").AsGuid().ForeignKey("appointments", "id")
                .WithColumn("date").AsDate().NotNullable()
                .WithColumn("time").AsInt32()
                .WithColumn("duration").AsInt32();
        }

        public override void Down() {
            Delete.Table("appointment_times");
        }
    }
}