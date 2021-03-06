using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_10_18_00, "Rename pad_sizes table to pad_series_sizes.")]
    public class RenamePadSizesToPadSeriesSizes : Migration {
        public override void Up() {
            Rename.Table("pad_sizes").To("pad_series_sizes");
            Delete.Column("pad_id").FromTable("pad_series_sizes");
            Alter.Table("pad_series_sizes").AddColumn("pad_series_id").AsGuid();
        }

        public override void Down() {
            Rename.Table("pad_series_sizes").To("pad_sizes");
            Delete.Column("pad_series_id").FromTable("pad_sizes");
            Alter.Table("pad_sizes").AddColumn("pad_id").AsGuid();
        }
    }
}