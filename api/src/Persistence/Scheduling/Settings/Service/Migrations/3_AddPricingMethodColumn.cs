using FluentMigrator;

namespace DetailingArsenal.Persistence.Settings.Migrations {
    [Migration(2020_04_18_00, "Add pricing_method column to services table.")]
    public class AddPricingMethodColumn : Migration {
        public override void Up() {
            Alter.Table("services").AddColumn("pricing_method").AsInt16();
        }

        public override void Down() {
            Delete.Column("pricing_method").FromTable("services");
        }
    }
}