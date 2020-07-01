using FluentMigrator;

namespace DetailingArsenal.Persistence.Billing.Migrations {
    [Migration(2020_06_28_1, "Create billing reference table")]
    public class CreateBillingReferenceTable : Migration {
        public override void Up() {
            Create.Table("billing_references")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("billing_id").AsString(255)
            .WithColumn("type").AsByte();
        }

        public override void Down() {
            Delete.Table("billing_references");
        }
    }
}