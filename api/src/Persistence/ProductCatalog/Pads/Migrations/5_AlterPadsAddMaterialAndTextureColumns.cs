using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2020_09_28_1, "Alter pads table add texture and material columns.")]
    public class AlterPadsAddMaterialAndTextureColumns : Migration {
        public override void Up() {
            Alter.Table("pads")
            .AddColumn("material").AsString(32)
            .AddColumn("texture").AsString(32);
        }

        public override void Down() {
            Delete
                .Column("material")
                .Column("texture")
                .FromTable("pads");
        }
    }
}