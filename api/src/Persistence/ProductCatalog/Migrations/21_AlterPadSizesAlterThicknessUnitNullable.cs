using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_12_25_02, "Alter pad sizes alter thickness unit nullable")]
    public class AlterPadSizesAlterThicknessUnitNullable : Migration {
        public override void Up() {
            Alter.Table("pad_sizes").AlterColumn("thickness_unit").AsString(2).Nullable();
        }

        public override void Down() {
            Alter.Table("pad_sizes").AlterColumn("thickness_unit").AsString(2);
        }
    }
}