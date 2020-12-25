using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_12_01_01, "Alter pad sizes table add measurement unit columns")]
    public class AlterPadSizesTableAddMeasurementUnitColumns : Migration {
        public override void Up() {
            Rename.Column("diameter").OnTable("pad_sizes").To("diameter_amount");
            Rename.Column("thickness").OnTable("pad_sizes").To("thickness_amount");

            Alter.Table("pad_sizes").AddColumn("diameter_unit").AsString(2);
            Alter.Table("pad_sizes").AddColumn("thickness_unit").AsString(2);
        }

        public override void Down() {
            Rename.Column("diameter_amount").OnTable("pad_sizes").To("diameter");
            Rename.Column("thickness_amount").OnTable("pad_sizes").To("thickness");

            Delete.Column("diameter_unit").FromTable("pad_sizes");
            Delete.Column("thickness_unit").FromTable("pad_sizes");
        }
    }
}