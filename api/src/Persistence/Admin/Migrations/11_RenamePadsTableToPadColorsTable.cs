using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_11_30_01, "Alter pads table to pad colors table")]
    public class RenamePadsTableToPadColorsTable : Migration {
        public override void Up() {
            Rename.Table("pads").To("pad_colors");

            Delete.Column("material")
                .Column("texture")
                .FromTable("pad_colors");
        }

        public override void Down() {
            Rename.Table("pad_colors").To("pads");

            Alter.Table("pad_colors")
            .AddColumn("material").AsString(32)
            .AddColumn("texture").AsString(32);
        }
    }
}