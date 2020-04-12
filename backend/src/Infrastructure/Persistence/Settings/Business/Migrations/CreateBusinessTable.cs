using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_02_26_0, "Create business table")]
    public class CreateBusinessTableMigration : Migration {
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