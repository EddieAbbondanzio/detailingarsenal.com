using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_07_04_00, "Alter subscription plan remove role id.")]
    public class AlterSubscriptionPlanRemoveRoleId : Migration {
        public override void Up() {
            Delete.Column("role_id").FromTable("subscription_plans");
        }

        public override void Down() {
            Alter.Table("subscription_plans").AddColumn("role_id").AsGuid().ForeignKey("roles", "id");
        }
    }
}