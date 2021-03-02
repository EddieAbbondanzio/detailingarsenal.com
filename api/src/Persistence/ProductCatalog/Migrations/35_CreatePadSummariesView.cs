using Dapper;
using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2021_03_02_00, "Create pad_summaries view")]
    public class CreatePadSummariesView : Migration {
        public override void Up() {
            Execute.WithConnection((c, o) => {
                c.Execute(@"
                    create view pad_summaries as 
                    select 
                        p.id, 
                        p.name, 
                        s.id as pad_series_id, s.name as pad_series_name,
                        b.id as brand_id, b.name as brand_name,
                        p.category,
                        p.material,
                        p.texture,
                        (select avg(r.cut) as cut from reviews r where r.pad_id = p.id),
                        (select avg(r.finish) as finish from reviews r where r.pad_id = p.id),
                        (select coalesce(avg(r.stars), 0) as stars from reviews r where r.pad_id = p.id),
                        (select count(r.*) as review_count from reviews r where r.pad_id = p.id),
                        s.polisher_types,
                        p.has_center_hole
                    from pads p 
                    left join pad_series s on p.pad_series_id = s.id
                    left join brands b on s.brand_id = b.id;
               ");
            });
        }

        public override void Down() {
            Execute.WithConnection((c, o) => {
                c.Execute("delete view pad_summaries");
            });
        }
    }
}