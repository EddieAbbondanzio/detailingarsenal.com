using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_07_01_00, "Alter customer table to reference new billing reference table")]
    public class AlterCustomerTableAddBillingReference : Migration {
        public override void Up() {
            Delete.Column("external_id").FromTable("customers");
            Alter.Table("customers").AddColumn("billing_reference_id").AsGuid().ForeignKey("billing_references", "id");
        }

        public override void Down() {
            Alter.Table("customers").AddColumn("external_id").AsGuid();
            Delete.Column("billing_reference_id").FromTable("customers");
        }
    }
}