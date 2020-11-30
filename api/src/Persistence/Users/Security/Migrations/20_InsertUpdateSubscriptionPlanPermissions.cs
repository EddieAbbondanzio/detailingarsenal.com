using DetailingArsenal.Persistence.Users.Security;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_11_29_01, "Insert update subscription plan permission")]
    public class InsertUpdateSubscriptionPlanPermission : Migration {
        public override void Up() {
            this.AddPermission("subscription-plans", "update");
        }

        public override void Down() {
            this.RemovePermissions("subscription-plans", "update");
        }
    }
}