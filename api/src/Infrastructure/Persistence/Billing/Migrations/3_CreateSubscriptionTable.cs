using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_06_11_1, "Create subscription table")]
    public class CreateSubscriptionsTable : Migration {
        public override void Up() {
            Create.Table("subscriptions")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
            .WithColumn("plan_id").AsGuid().ForeignKey("subscription_plans", "id")
            .WithColumn("external_id").AsString(255)
            .WithColumn("status").AsString(32);
        }

        public override void Down() {
            Delete.Table("subscriptions");
        }
    }
}