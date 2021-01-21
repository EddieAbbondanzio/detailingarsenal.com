using FluentMigrator;

namespace DetailingArsenal.Persistence.Settings.Migrations {
    [Migration(2020_02_26_00, "Create business table")]
    public class CreateBusinessTable : Migration {
        public override void Up() {
            Create.Table("businesses")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("name").AsString(32).Nullable()
                .WithColumn("address").AsString(128).Nullable()
                .WithColumn("phone").AsString(32).Nullable();
        }

        public override void Down() {
            Delete.Table("businesses");
        }
    }
}