using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_01_31_00, "Delete image id column from pads table.")]
    public class DeleteImageIdColumnFromPadsTable : Migration {
        public override void Up() {
            Delete.Column("image_id").FromTable("pads");
        }

        public override void Down() {
            Create.Column("image_id").OnTable("pads").AsGuid().ForeignKey("id", "images").Nullable();
        }
    }
}