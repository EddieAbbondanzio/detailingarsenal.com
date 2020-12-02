using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_08_24_0, "Create pad_series table")]
    public class CreatePadSeriesTable : Migration {
        public override void Up() {
            Create.Table("pad_series")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("brand_id").AsGuid().ForeignKey("brands", "id")
                .WithColumn("name").AsString(32);
        }

        public override void Down() {
            Delete.Table("pad_series");
        }
    }
}