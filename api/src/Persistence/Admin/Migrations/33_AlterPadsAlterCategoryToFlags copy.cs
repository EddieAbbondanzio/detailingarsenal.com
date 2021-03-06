using Dapper;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_02_27_00, "Alter pads alter category to support enum flags")]
    public class AlterPadsAlterCategoryToFlags : Migration {
        public override void Up() {
            Execute.WithConnection((c, t) => {
                c.Execute(@"
                alter table pads alter column category type integer using 
                    (case
                        when category = 'cutting' then 1
                        when category = 'polishing' then 2
                        when category = 'finishing' then 4
                    end);
                ");
            });
        }

        public override void Down() {
            Execute.WithConnection((c, t) => {
                c.Execute(@"
                alter table pads alter column category type varchar(32) using 
                    (case
                        when category = 1 then 'cutting'
                        when category = 2 then 'polishing'
                        when category > 2 then 'finishing'
                    end);
                ");
            });
        }
    }
}