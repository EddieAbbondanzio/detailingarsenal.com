using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_07_27_00, "Alter payment_methods add billing_reference_id")]
    public class AlterPaymentMethodsAddBillingReferenceId : Migration {
        public override void Up() {
            Alter.Table("payment_methods").AddColumn("billing_reference_id").AsGuid().ForeignKey("billing_references", "id");
        }

        public override void Down() {
            Delete.Column("billing_reference_id").FromTable("payment_methods");
        }
    }
}