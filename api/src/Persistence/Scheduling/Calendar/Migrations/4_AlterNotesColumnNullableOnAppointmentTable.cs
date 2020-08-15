using FluentMigrator;

namespace DetailingArsenal.Persistence.Calendar.Migrations {
    [Migration(2020_03_31_1, "Alter notes column nullable on appointment table")]
    public class AlterNotesNullableOnAppointmentTable : Migration {
        public override void Up() {
            Alter.Column("notes").OnTable("appointments").AsString(1024).Nullable();
        }

        public override void Down() {
            Alter.Column("notes").OnTable("appointments").AsString(1024).NotNullable();
        }
    }
}