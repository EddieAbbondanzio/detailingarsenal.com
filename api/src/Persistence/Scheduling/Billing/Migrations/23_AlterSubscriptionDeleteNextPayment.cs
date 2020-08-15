using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_08_08_00, "Alter subscriptions delete next_payment, add period_start, period_end.")]
    public class AlterSubscriptionsDeleteNextPayment : Migration {
        public override void Up() {
            Delete.Column("next_payment").FromTable("subscriptions");

            Alter.Table("subscriptions")
                .AddColumn("period_start").AsDateTime()
                .AddColumn("period_end").AsDateTime();
        }

        public override void Down() {
            Delete
                .Column("period_start")
                .Column("period_end")
                .FromTable("subscriptions");

            Alter.Table("subscriptions").AddColumn("next_payment").AsDateTime().Nullable();
        }
    }
}