using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_06_28_2, "Alter subscription plan table to reference new billing reference table")]
    public class AlterSubscriptionPlanTableAddBillingReference : Migration {
        public override void Up() {
            Delete.Column("external_id").FromTable("subscription_plans");
            Alter.Table("subscription_plans").AddColumn("billing_reference_id").AsGuid().ForeignKey("billing_references", "id");
        }

        public override void Down() {
            Alter.Table("subscription_plans").AddColumn("external_id").AsGuid();
            Delete.Column("billing_reference_id").FromTable("subscription_plans");
        }
    }
}