using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_08_24_01, "Create pads table")]
    public class CreatePadsTable : Migration {
        public override void Up() {
            Create.Table("pads")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("pad_series_id").AsGuid().ForeignKey("pad_series", "id")
                .WithColumn("category").AsInt16()
                .WithColumn("name").AsString(32)
                .WithColumn("image").AsBinary().Nullable(); // AsBinary() uses BYTEA in Postgres
        }

        public override void Down() {
            Delete.Table("pads");
        }
    }
}