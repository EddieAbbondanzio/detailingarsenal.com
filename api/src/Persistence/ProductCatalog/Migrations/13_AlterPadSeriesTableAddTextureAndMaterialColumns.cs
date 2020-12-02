using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_11_30_02, "Alter pad_series table add texture and material")]
    public class AlterPadSeriesTableAddTextureAndMaterialColumns : Migration {
        public override void Up() {
            Alter.Table("pad_series")
            .AddColumn("material").AsString(32).Nullable()
            .AddColumn("texture").AsString(32).Nullable();
        }

        public override void Down() {
            Delete.Column("material").Column("texture").FromTable("pad_series");
        }
    }
}