using FluentMigrator;

namespace DetailingArsenal.Persistence.Shared.Migrations {
    [Migration(2021_01_16_00, "Alter images table remove parent_type column.")]
    public class AlterImagesTableRemoveParentTypeColumn : Migration {
        public override void Up() {
            Delete.Column("parent_type").FromTable("images");
        }

        public override void Down() {
            Create.Column("parent_type").OnTable("images").AsString(16);
        }
    }
}