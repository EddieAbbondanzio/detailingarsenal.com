using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_07_26_00, "Alter subscription add cancelling at period end column")]
    public class AlterSubscriptionAddCancellingAtPeriodEndFlag : Migration {
        public override void Up() {
            Alter.Table("subscriptions").AddColumn("cancelling_at_period_end").AsBoolean().WithDefaultValue(false);
        }

        public override void Down() {
            Delete.Column("cancelling_at_period_end").FromTable("subscriptions");
        }
    }
}