using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_09_28_00, "Create pad-size table")]
    public class CreatePadSizesTable : Migration {
        public override void Up() {
            Create.Table("pad_sizes")
            .WithColumn("pad_id").AsGuid().ForeignKey("pads", "id")
            .WithColumn("diameter").AsFloat()
            .WithColumn("thickness").AsFloat()
            .WithColumn("part_number").AsString(64);
        }

        public override void Down() {
            Delete.Table("pad_sizes");
        }
    }
}