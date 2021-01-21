using FluentMigrator;

namespace DetailingArsenal.Persistence.Settings.Migrations {
    [Migration(2020_03_22_01, "Create service_configurations table")]
    public class CreateServiceConfigurationsTableMigration : Migration {
        public override void Up() {
            Create.Table("service_configurations")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("service_id").AsGuid().ForeignKey("services", "id")
                .WithColumn("vehicle_category_id").AsGuid().ForeignKey("vehicle_categories", "id")
                .WithColumn("price").AsDecimal().NotNullable()
                .WithColumn("duration").AsInt32();
        }

        public override void Down() {
            Delete.Table("service_configurations");
        }
    }
}