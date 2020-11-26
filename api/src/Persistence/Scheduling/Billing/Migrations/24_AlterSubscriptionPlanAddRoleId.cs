using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_11_26_00, "Add Role Id to Subscription Plan")]
    public class AlterSubscriptionPlanAddRoleId : Migration {
        public override void Up() {
            Alter.Table("subscription_plans").AddColumn("role_id").AsGuid().ForeignKey("roles", "id").Nullable();
        }

        public override void Down() {
            Delete.Column("role_id").FromTable("subscription_plans");
        }
    }
}