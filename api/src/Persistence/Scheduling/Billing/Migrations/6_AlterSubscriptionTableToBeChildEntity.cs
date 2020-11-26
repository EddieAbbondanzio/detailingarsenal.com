using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_06_28_0, "Alter subscription table to be child entity")]
    public class AlterSubscriptionTableToBeChildEntity : Migration {
        public override void Up() {
            Delete.Column("user_id").FromTable("subscriptions");

            Alter.Table("subscriptions")
            .AddColumn("customer_id").AsGuid().ForeignKey("customers", "id");
        }

        public override void Down() {
            Alter.Table("subscriptions").AddColumn("user_id").AsGuid().ForeignKey("user", "id");
            Delete.Column("customer_id").FromTable("subscriptions");

        }
    }
}