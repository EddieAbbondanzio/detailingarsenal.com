using FluentMigrator;

namespace DetailingArsenal.Persistence.Calendar.Migrations {
    /// <summary>
    /// "end" is a reserved keyword in postgres.
    /// </summary>
    [Migration(2020_04_24_0, "Alter start and end columns to time stamps")]
    public class AlterStartAndEndToTimestamps : Migration {
        public override void Up() {
            Alter.Column("start_date").OnTable("appointment_blocks").AsDateTime();
            Alter.Column("end_date").OnTable("appointment_blocks").AsDateTime();
        }

        public override void Down() {
            Alter.Column("start_date").OnTable("appointment_blocks").AsDate();
            Alter.Column("end_date").OnTable("appointment_blocks").AsDate();
        }
    }
}