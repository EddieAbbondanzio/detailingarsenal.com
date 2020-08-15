using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_07_13_01, "Alter subscription add next payment date column")]
    public class AlterSubscriptionAddNextPayment : Migration {
        public override void Up() {
            Alter.Table("subscriptions").AddColumn("next_payment").AsDateTime().Nullable();
        }

        public override void Down() {
            Delete.Column("payment_method").FromTable("subscriptions");
        }
    }
}