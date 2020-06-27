using FluentMigrator;

namespace DetailingArsenal.Persistence.Calendar.Migrations {
    /// <summary>
    /// "end" is a reserved keyword in postgres.
    /// </summary>
    [Migration(2020_04_23_0, "Rename start and end columns since end is a reserved keyword in Postgres.")]
    public class RenameStartAndEndColumns : Migration {
        public override void Up() {
            Rename.Column("start").OnTable("appointment_blocks").To("start_date");
            Rename.Column("end").OnTable("appointment_blocks").To("end_date");
        }

        public override void Down() {
            Rename.Column("start_date").OnTable("appointment_blocks").To("start");
            Rename.Column("end_date").OnTable("appointment_blocks").To("end");
        }
    }
}