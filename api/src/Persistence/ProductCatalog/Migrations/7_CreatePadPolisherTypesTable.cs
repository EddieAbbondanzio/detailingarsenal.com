using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_09_28_02, "Create pad polisher types table.")]
    public class CreatePadPolisherTypesTable : Migration {
        public override void Up() {
            Create.Table("pad_polisher_types")
            .WithColumn("pad_id").AsGuid().ForeignKey("pads", "id")
            .WithColumn("polisher_type").AsString(32);
        }

        public override void Down() {
            Delete.Table("pad_polisher_types");
        }
    }
}