using FluentMigrator;

namespace DetailingArsenal.Persistence.Scheduling.Billing.Migrations {
    [Migration(2020_06_28_01, "Create billing reference table")]
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