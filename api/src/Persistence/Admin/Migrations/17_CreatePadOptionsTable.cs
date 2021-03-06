using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_11_30_06, "Create pad_options table.")]
    public class CreatePadOptionsTable : Migration {
        public override void Up() {
            Create.Table("pad_options")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("pad_color_id").AsGuid().ForeignKey("pad_colors", "id")
            .WithColumn("pad_size_id").AsGuid().ForeignKey("pad_sizes", "id")
            .WithColumn("part_number").AsString(64).Nullable();

            Create.UniqueConstraint("unique_pad_options").OnTable("pad_options").Columns("pad_color_id", "pad_size_id");
        }

        public override void Down() {
            Delete.Table("pad_options");
        }
    }
}