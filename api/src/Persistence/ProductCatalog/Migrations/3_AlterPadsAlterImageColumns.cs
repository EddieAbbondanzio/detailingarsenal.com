using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_08_26_00, "Alter pads table change image to image_name, and image_data")]
    public class AlterPadsAlterImageColumns : Migration {
        public override void Up() {
            Delete.Column("image").FromTable("pads");
            Alter.Table("pads")
                .AddColumn("image_name").AsString(128).Nullable()
                .AddColumn("image_data").AsBinary().Nullable();
        }

        public override void Down() {
            Delete.Column("image_name").Column("image_data").FromTable("pads");
            Alter.Table("pads").AddColumn("image").AsBinary();
        }
    }
}