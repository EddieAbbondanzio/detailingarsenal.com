using FluentMigrator;

namespace DetailingArsenal.Persistence.Calendar.Migrations {
    [Migration(2020_04_22_0, "Alter appointment_blocks to store using start, and end date.")]
    public class StartAndEndDateAppointmentBlocks : Migration {
        public override void Up() {
            Alter.Table("appointment_blocks").AddColumn("start").AsDate();
            Alter.Table("appointment_blocks").AddColumn("end").AsDate();

            Delete.Column("date").FromTable("appointment_blocks");
            Delete.Column("time").FromTable("appointment_blocks");
            Delete.Column("duration").FromTable("appointment_blocks");
        }

        public override void Down() {
            Delete.Column("start").FromTable("appointment_blocks");
            Delete.Column("end").FromTable("appointment_blocks");

            Alter.Table("appointment_blocks").AddColumn("date").AsDate();
            Alter.Table("appointment_blocks").AddColumn("time").AsInt32();
            Alter.Table("appointment_blocks").AddColumn("duration").AsInt32();
        }
    }
}