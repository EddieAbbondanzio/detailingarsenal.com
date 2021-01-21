using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_11_17_00, "Create reviews table")]
    public class CreateReviewsTable : Migration {
        public override void Up() {
            Create.Table("reviews")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("pad_id").AsGuid().ForeignKey("pads", "id")
                .WithColumn("created_date").AsDateTime()
                .WithColumn("stars").AsByte()
                .WithColumn("cut").AsByte().Nullable()
                .WithColumn("finish").AsByte().Nullable()
                .WithColumn("title").AsString(64)
                .WithColumn("body").AsString(10_000);
        }

        public override void Down() {
            Delete.Table("reviews");
        }
    }
}