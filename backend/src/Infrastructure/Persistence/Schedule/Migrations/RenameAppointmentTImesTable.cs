using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_03_29_2, "Rename appointment_times table")]
    public class RenameAppointmentTimesTable : Migration {
        public override void Up() {
            Rename.Table("appointment_times").To("appointment_blocks");
        }

        public override void Down() {
            Rename.Table("appointment_blocks").To("appointment_times");
        }
    }
}