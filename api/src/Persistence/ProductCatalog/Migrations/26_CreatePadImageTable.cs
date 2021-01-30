using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2021_01_29_00, "Create pad_image table.")]
    public class CreatePadImageTable : Migration {
        public override void Up() {
            Create.Table("pad_images")
             .WithColumn("pad_id").AsGuid().ForeignKey("pads", "id")
             .WithColumn("image_id").AsGuid().ForeignKey("images", "id");

            Create.UniqueConstraint().OnTable("pad_images").Columns("pad_id", "image_id");
        }

        public override void Down() {
            Delete.Table("pad_images");
        }
    }
}