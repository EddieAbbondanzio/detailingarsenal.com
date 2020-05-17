using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_03_27_0, "Create clients table")]
    public class CreateClientTable : Migration {
        public override void Up() {
            Create.Table("clients")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("name").AsString(64).NotNullable()
                .WithColumn("phone").AsString(32).Nullable()
                .WithColumn("email").AsString(256 + 64).Nullable();
        }

        public override void Down() {
            Delete.Table("clients");
        }
    }
}