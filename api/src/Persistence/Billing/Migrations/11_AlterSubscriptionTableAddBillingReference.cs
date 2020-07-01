using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_07_01_1, "Alter customer table to reference new billing reference table")]
    public class AlterSubscriptionTableAddBillingReference : Migration {
        public override void Up() {
            Delete.Column("external_id").FromTable("subscriptions");
            Alter.Table("subscriptions")
                .AddColumn("billing_reference_id").AsGuid().ForeignKey("billing_references", "id");
        }

        public override void Down() {
            Alter.Table("subscriptions").AddColumn("external_id").AsGuid();
            Delete.Column("billing_reference_id").FromTable("subscriptions");
        }
    }
}