using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_06_29_0, "Alter subscription plan prices table to reference new billing reference table")]
    public class AlterSubscriptionPlanPriceTableAddBillingReference : Migration {
        public override void Up() {
            Delete.Column("external_id").FromTable("subscription_plan_prices");
            Alter.Table("subscription_plan_prices").AddColumn("billing_reference_id").AsGuid().ForeignKey("billing_references", "id");
        }

        public override void Down() {
            Alter.Table("subscription_plan_prices").AddColumn("external_id").AsGuid();
            Delete.Column("billing_reference_id").FromTable("subscription_plan_prices");
        }
    }
}