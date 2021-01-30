using Dapper;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Shared.Migrations {
    [Migration(2021_01_29_01, "Delete parent id column from images table.")]
    public class DeleteParentIdColumnFromImagesTable : Migration {
        public override void Up() {
            // Move pad image references to their new table
            Execute.WithConnection((c, t) => {
                var images = c.Query("select * from images;");

                foreach (var image in images) {
                    c.Execute("insert into pad_images (pad_id, image_id) values (@PadId, @ImageId);", new {
                        PadId = image.parent_id,
                        ImageId = image.id
                    });
                }
            });

            Delete.Column("parent_id").FromTable("images");
        }

        public override void Down() {
            Create.Column("parent_id").OnTable("images").AsGuid();
        }
    }
}