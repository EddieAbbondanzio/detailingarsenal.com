using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_01_30_00, "Add has_center_hole to pads table.")]
    public class AddHasCenterHoleColumnToPadsTable : Migration {
        public override void Up() {
            Create.Column("has_center_hole").OnTable("pads").AsBoolean().Nullable();
        }

        public override void Down() {
            Delete.Column("has_center_hole").FromTable("pads");
        }
    }
}