using FluentMigrator;

namespace DetailingArsenal.Persistence.Calendar.Migrations {
    [Migration(2020_03_29_00, "Create appointments table")]
    public class CreateAppointmentTable : Migration {
        public override void Up() {
            Create.Table("appointments")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("service_id").AsGuid().ForeignKey("services", "id")
                .WithColumn("vehicle_category_id").AsGuid().ForeignKey("vehicle_categories", "id")
                .WithColumn("client_id").AsGuid().ForeignKey("clients", "id")
                .WithColumn("price").AsDecimal()
                .WithColumn("duration").AsInt32()
                .WithColumn("notes").AsString(1024);
        }

        public override void Down() {
            Delete.Table("appointments");
        }
    }
}