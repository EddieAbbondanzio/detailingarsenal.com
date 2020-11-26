using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_07_08_1, "Alter subscription plans add description")]
    public class AlterSubscriptionPlanTableAddDescription : Migration {
        public override void Up() {
            Alter.Table("subscription_plans").AddColumn("description").AsString(1024).Nullable();
        }

        public override void Down() {
            Delete.Column("description").FromTable("subscription_plans");
        }
    }
}