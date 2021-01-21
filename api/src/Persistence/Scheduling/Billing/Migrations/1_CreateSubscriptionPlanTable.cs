using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_06_08_00, "Create subscription plans table")]
    public class CreateSubscriptionPlansTable : Migration {
        public override void Up() {
            Create.Table("subscription_plans")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString(128).Unique()
                .WithColumn("external_id").AsString(255).Unique();
        }

        public override void Down() {
            Delete.Table("subscription_plans");
        }
    }
}