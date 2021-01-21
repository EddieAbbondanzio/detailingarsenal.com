using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_08_18_00, "Create brands table")]
    public class CreateBrandsTable : Migration {
        public override void Up() {
            Create.Table("brands")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString(32);
        }

        public override void Down() {
            Delete.Table("brands");
        }
    }
}