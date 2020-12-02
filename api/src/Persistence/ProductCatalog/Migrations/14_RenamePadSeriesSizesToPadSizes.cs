using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_11_30_03, "Rename pad series sizes to pad sizes")]
    public class RenamePadSeriesSizesToPadSizes : Migration {
        public override void Up() {
            Delete.Column("part_number").FromTable("pad_series_sizes");
            Rename.Table("pad_series_sizes").To("pad_sizes");
        }

        public override void Down() {
            Alter.Table("pad_series_sizes").AddColumn("part_number").AsString(64).Nullable();
            Rename.Table("pad_sizes").To("pad_series_sizes");
        }
    }
}