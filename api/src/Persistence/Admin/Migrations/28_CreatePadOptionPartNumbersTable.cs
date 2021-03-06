using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_01_29_03, "Create pad_option_part_numbers table.")]
    public class CreatePadOptionPartNumbersTable : Migration {
        public override void Up() {
            Create.Table("pad_option_part_numbers")
             .WithColumn("pad_option_id").AsGuid().ForeignKey("pad_options", "id")
             .WithColumn("part_number_id").AsGuid().ForeignKey("part_numbers", "id");

            Create.UniqueConstraint().OnTable("pad_option_part_numbers").Columns("pad_option_id", "part_number_id");
        }

        public override void Down() {
            Delete.Table("pad_option_part_numbers");
        }
    }
}