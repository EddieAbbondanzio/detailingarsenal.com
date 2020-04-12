using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_03_18_0, "Create employees table")]
    public class CreateEmployeeTableMigration : Migration {
        public override void Up() {
            Create.Table("employees")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("name").AsString(32).NotNullable()
                .WithColumn("position").AsString(128);
        }

        public override void Down() {
            Delete.Table("employees");
        }
    }
}