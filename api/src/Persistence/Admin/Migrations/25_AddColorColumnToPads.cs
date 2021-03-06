using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_01_27_01, "Add color column to pads table")]
    public class AddColorColumnToPadsTable : Migration {
        public override void Up() {
            Alter.Table("pads").AddColumn("color").AsString(16).Nullable();
            Alter.Column("material").OnTable("pads").AsString(32).Nullable();
            Alter.Column("texture").OnTable("pads").AsString(32).Nullable();
        }

        public override void Down() {
            Delete.Column("color").FromTable("pads");
            Alter.Column("material").OnTable("pads").AsString(32).NotNullable();
            Alter.Column("texture").OnTable("pads").AsString(32).NotNullable();
        }
    }
}