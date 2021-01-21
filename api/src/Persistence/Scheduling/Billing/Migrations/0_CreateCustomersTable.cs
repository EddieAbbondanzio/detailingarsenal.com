using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_06_07_00, "Create customers table")]
    public class CreateCustomersTable : Migration {
        public override void Up() {
            Create.Table("customers")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("external_id").AsString(255);
        }

        public override void Down() {
            Delete.Table("customers");
        }
    }
}