using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2021_01_29_02, "Create part number table.")]
    public class CreatePartNumberTable : Migration {
        public override void Up() {
            Create.Table("part_numbers")
             .WithColumn("id").AsGuid().PrimaryKey()
             .WithColumn("value").AsString(64)
             .WithColumn("notes").AsString(128).Nullable();
        }

        public override void Down() {
            Delete.Table("part_numbers");
        }
    }
}