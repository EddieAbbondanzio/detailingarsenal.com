using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_09_27_00, "Alter pads change category to string")]
    public class AlterPadsAlterCategoryAsString : Migration {
        public override void Up() {
            Alter.Table("pads").AlterColumn("category").AsString(32);
        }

        public override void Down() {
            Alter.Table("pads").AlterColumn("category").AsInt16();
        }
    }
}