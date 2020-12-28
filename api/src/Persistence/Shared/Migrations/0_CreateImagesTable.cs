using FluentMigrator;

namespace DetailingArsenal.Persistence.Shared.Migrations {
    [Migration(2020_12_27_01, "Create images table")]
    public class CreateImagesTable : Migration {
        public override void Up() {
            Create.Table("images")
                .WithColumn("id").AsGuid()
                .WithColumn("parent_id").AsGuid()
                .WithColumn("parent_type").AsString(16)
                .WithColumn("file_name").AsString(128)
                .WithColumn("mime_type").AsString(128)
                .WithColumn("image_data").AsBinary()
                .WithColumn("thumbnail_data").AsBinary();
        }

        public override void Down() {
            Delete.Table("images");
        }
    }
}