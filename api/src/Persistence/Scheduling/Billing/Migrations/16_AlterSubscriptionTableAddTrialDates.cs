using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_07_09_01, "Alter subscription plans add trial start and trial end")]
    public class AlterSubscriptionAddTrialDates : Migration {
        public override void Up() {
            Alter.Table("subscriptions").AddColumn("trial_start").AsDateTime()
            .AddColumn("trial_end").AsDateTime();
        }

        public override void Down() {
            Delete.Column("trial_start").FromTable("subscriptions");
            Delete.Column("trial_end").FromTable("subscriptions");
        }
    }
}