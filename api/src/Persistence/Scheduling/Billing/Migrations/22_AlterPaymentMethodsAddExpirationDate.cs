using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_07_31_00, "Alter payment_methods add expiration month, expiration year.")]
    public class AlterPaymentMethodsAddExpiration : Migration {
        public override void Up() {
            Alter.Table("payment_methods")
                .AddColumn("expiration_month").AsString(2).WithDefaultValue("00")
                .AddColumn("expiration_year").AsString(4).WithDefaultValue("0000");
        }

        public override void Down() {
            Delete.Column("expiration_month").Column("expiration_year").FromTable("payment_methods");
        }
    }
}