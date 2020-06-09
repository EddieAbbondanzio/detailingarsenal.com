using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_06_08_0, "Create subscription plans table")]
    public class CreateSubscriptionPlansTable : Migration {
        public override void Up() {
            Create.Table("subscription_plans")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("external_id").AsString(255);
        }

        public override void Down() {
            Delete.Table("subscription_plans");
        }
    }
}