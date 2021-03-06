using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_11_30_08, "Alter reviews table alter pad id to pad color id")]
    public class AlterReviewsTableAlterPadIdToPadColorId : Migration {
        public override void Up() {
            Delete.Column("pad_id").FromTable("reviews");
            Alter.Table("reviews").AddColumn("pad_color_id").AsGuid().ForeignKey("pad_colors", "id");
        }

        public override void Down() {
            Delete.Column("pad_color_id").FromTable("reviews");
            Alter.Table("reviews").AddColumn("pad_id").AsGuid().ForeignKey("pads", "id");
        }
    }
}