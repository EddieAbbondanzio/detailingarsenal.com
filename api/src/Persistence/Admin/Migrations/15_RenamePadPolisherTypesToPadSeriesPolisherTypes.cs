using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_11_30_04, "Rename pad polisher types to pad series polisher types")]
    public class RenamePadPolisherTypesToPadSeriesPolisherTypes : Migration {
        public override void Up() {
            Delete.Table("pad_polisher_types");
            Create.Table("pad_series_polisher_types")
                .WithColumn("pad_series_id").AsGuid().ForeignKey("pad_series", "id")
                .WithColumn("polisher_type").AsString(32);
        }

        public override void Down() {
            Delete.Table("pad_series_polisher_types");
            Create.Table("pad_polisher_types")
                .WithColumn("pad_id").AsGuid().ForeignKey("pad", "id")
                .WithColumn("polisher_type").AsString(32);
        }
    }
}