using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_01_26_00, "Move material and texture column back to pad color")]
    public class MovePadMaterialAndTextureToPadColor : Migration {
        public override void Up() {
            Delete.Column("material").Column("texture").FromTable("pad_series");
            Alter.Table("pad_colors").AddColumn("material").AsString(32).AddColumn("texture").AsString(32);
        }

        public override void Down() {
            Delete.Column("material").Column("texture").FromTable("pad_colors");
            Alter.Table("pad_series").AddColumn("material").AsString(32).AddColumn("texture").AsString(32);
        }
    }
}