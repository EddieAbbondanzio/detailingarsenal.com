using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_03_22_0, "Create services table")]
    public class CreateServicesTableMigration : Migration {
        public override void Up() {
            Create.Table("services")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("name").AsString(32).NotNullable()
                .WithColumn("description").AsString(512);
        }

        public override void Down() {
            Delete.Table("services");
        }
    }
}