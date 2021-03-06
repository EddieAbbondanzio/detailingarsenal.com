using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_12_27_02, "Alter pad colors externalize image data")]
    public class AlterPadColorsExternalizeImageColumns : Migration {
        public override void Up() {
            Delete.Column("image_name").Column("image_data").FromTable("pad_colors");

            Alter.Table("pad_colors").AddColumn("image_id").AsGuid().ForeignKey("images", "id").Nullable();
        }

        public override void Down() {
            Delete.Column("image_id").FromTable("pad_colors");

            Alter.Table("pad_colors")
                .AddColumn("image_name").AsString(128).Nullable()
                .AddColumn("image_data").AsBinary().Nullable();
        }
    }
}