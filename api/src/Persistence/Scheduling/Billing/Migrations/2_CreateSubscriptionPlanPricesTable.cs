using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_06_08_1, "Create subscription plan prices table")]
    public class CreateSubscriptionPlanPricesTable : Migration {
        public override void Up() {
            Create.Table("subscription_plan_prices")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("plan_id").AsGuid().ForeignKey("subscription_plans", "id")
                .WithColumn("price").AsDecimal()
                .WithColumn("interval").AsString(32)
                .WithColumn("external_id").AsString(255).Unique();
        }

        public override void Down() {
            Delete.Table("subscription_plan_prices");
        }
    }
}