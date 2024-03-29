using FluentMigrator;

namespace DetailingArsenal.Persistence.Settings.Migrations {
    [Migration(2020_02_24_00, "Create vehicle_categories table")]
    public class CreateVehicleCategoryTable : Migration {
        public override void Up() {
            Create.Table("vehicle_categories")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("name").AsString(32).NotNullable()
                .WithColumn("description").AsString(128);
        }

        public override void Down() {
            Delete.Table("vehicle_categories");
        }
    }
}