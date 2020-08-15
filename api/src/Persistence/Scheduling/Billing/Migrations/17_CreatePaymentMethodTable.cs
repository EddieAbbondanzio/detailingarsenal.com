using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_07_13_00, "Create payment method table")]
    public class CreatePaymentMethodTable : Migration {
        public override void Up() {
            Create.Table("payment_methods")
                .WithColumn("id").AsGuid()
                .WithColumn("customer_id").AsGuid().ForeignKey("customers", "id")
                .WithColumn("brand").AsString(32)
                .WithColumn("last_4").AsString(4);
        }

        public override void Down() {
            Delete.Table("payment_methods");
        }
    }
}