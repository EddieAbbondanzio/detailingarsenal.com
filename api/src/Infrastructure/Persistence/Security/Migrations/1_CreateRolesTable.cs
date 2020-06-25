using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_05_29_0, "Create roles table")]
    public class CreateRolesTable : Migration {
        public override void Up() {
            Create.Table("roles")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString(32);
        }

        public override void Down() {
            Delete.Table("roles");
        }
    }
}