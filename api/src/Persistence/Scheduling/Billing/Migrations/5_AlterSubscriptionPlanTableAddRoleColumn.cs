using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_06_11_02, "Alter subscription plan table add role id column")]
    public class AlterSubscriptionPlanTableAddUserId : Migration {
        public override void Up() {
            Alter.Table("subscription_plans")
            .AddColumn("role_id").AsGuid().ForeignKey("roles", "id").Nullable();
        }

        public override void Down() {
            Delete.Column("role_id").FromTable("subscription_plans");
        }
    }
}