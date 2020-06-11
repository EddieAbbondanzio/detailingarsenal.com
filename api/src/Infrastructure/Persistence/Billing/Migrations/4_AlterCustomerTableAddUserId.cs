using FluentMigrator;

namespace DetailingArsenal.Infrastructure.Persistence.Migrations {
    [Migration(2020_06_11_0, "Alter customers table add user id column")]
    public class AlterCustomerTableAddUserId : Migration {
        public override void Up() {
            Alter.Table("customers")
            .AddColumn("user_id").AsGuid().ForeignKey("users", "id");
        }

        public override void Down() {
            Delete.Column("user_id").FromTable("customers");
        }
    }
}