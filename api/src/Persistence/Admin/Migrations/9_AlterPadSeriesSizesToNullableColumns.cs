using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_10_19_00, "Alter pad_series_sizes to have nullable columns.")]
    public class AlterPadSeriesSizesToNullableColumns : Migration {
        public override void Up() {
            Alter.Column("thickness").OnTable("pad_series_sizes").AsFloat().Nullable();
            Alter.Column("part_number").OnTable("pad_series_sizes").AsString(64).Nullable();
        }

        public override void Down() {
            Alter.Column("thickness").OnTable("pad_series_sizes").AsFloat().NotNullable();
            Alter.Column("part_number").OnTable("pad_series_sizes").AsString(64).NotNullable();
        }
    }
}