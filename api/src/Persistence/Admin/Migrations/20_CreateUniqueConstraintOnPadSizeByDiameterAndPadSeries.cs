using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_12_25_01, "Create unique constraint on pad size by diameter and pad series id")]
    public class CreateUniqueConstraintOnPadSizeByDiameterAndPadSeries : Migration {
        public override void Up() {
            Create.UniqueConstraint("pad_sizes_unique_by_diameter_and_series").OnTable("pad_sizes").Columns("diameter_amount", "diameter_unit", "pad_series_id");
        }

        public override void Down() {
            Delete.UniqueConstraint("pad_sizes_unique_by_diameter_and_series").FromTable("pad_sizes");
        }
    }
}