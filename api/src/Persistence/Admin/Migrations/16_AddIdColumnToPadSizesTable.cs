using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2020_11_30_05, "Add Id column to pad sizes table")]
    public class AddIdColumnToPadSizesTable : Migration {
        public override void Up() {
            Alter.Table("pad_sizes").AddColumn("id").AsGuid();
            Create.PrimaryKey("PK_pad_sizes").OnTable("pad_sizes").Column("id");
        }

        public override void Down() {
            Delete.Column("id").FromTable("pad_sizes");
        }
    }
}