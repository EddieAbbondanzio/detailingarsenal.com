using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_01_27_00, "Rename pad colors table to pads")]
    public class RenamePadColorsTableToPads : Migration {
        public override void Up() {
            Rename.Table("pad_colors").To("pads");
            Rename.Column("pad_color_id").OnTable("pad_options").To("pad_id");
            Rename.Column("pad_color_id").OnTable("reviews").To("pad_id");
        }

        public override void Down() {
            Rename.Table("pads").To("pad_colors");
            Rename.Column("pad_id").OnTable("pad_options").To("pad_color_id");
            Rename.Column("pad_id").OnTable("reviews").To("pad_color_id");
        }
    }
}