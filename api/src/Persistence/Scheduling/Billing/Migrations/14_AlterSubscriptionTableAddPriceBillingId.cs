using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_07_08_00, "Alter subscription add price billing id.")]
    public class AlterSubscriptionTableAddPriceBillingId : Migration {
        public override void Up() {
            Alter.Table("subscriptions").AddColumn("price_billing_id").AsString(255);
        }

        public override void Down() {
            Delete.Column("price_billing_id").FromTable("subscriptions");
        }
    }
}