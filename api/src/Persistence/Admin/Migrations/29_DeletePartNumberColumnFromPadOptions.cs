using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_01_29_07, "Delete part_number column from pad_options")]
    public class DeletePartNumberColumnFromPadOptions : Migration {
        public override void Up() {
            Delete.Column("part_number").FromTable("pad_options");
        }

        public override void Down() {
            Create.Column("part_number").OnTable("pad_options").AsString(64).Nullable();
        }
    }
}