using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_07_28_00, "Alter payment_methods add is default")]
    public class AlterPaymentMethodsAddIsDefault : Migration {
        public override void Up() {
            Alter.Table("payment_methods").AddColumn("is_default").AsBoolean();
        }

        public override void Down() {
            Delete.Column("is_default").FromTable("payment_methods");
        }
    }
}