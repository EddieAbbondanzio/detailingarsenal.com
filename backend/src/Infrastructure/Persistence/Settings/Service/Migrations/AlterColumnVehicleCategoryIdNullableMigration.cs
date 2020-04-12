using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_04_07_0, "Alter column vehicle_category_id as nullable service_configurations table.")]
    public class AlterColumnVehicleCategoryIdNullableMigration : Migration {
        public override void Up() {
            Alter.Table("service_configurations").AlterColumn("vehicle_category_id").AsGuid().Nullable();
        }

        public override void Down() {
            Alter.Table("service_configurations").AlterColumn("vehicle_category_id").AsGuid();
        }
    }
}