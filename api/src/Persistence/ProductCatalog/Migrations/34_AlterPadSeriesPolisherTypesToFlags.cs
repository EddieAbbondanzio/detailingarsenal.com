using System;
using System.Collections.Generic;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;
using FluentMigrator;

namespace DetailingArsenal.Persistence.ProductCatalog.Migrations {
    [Migration(2021_02_28_00, "Alter pad series polisher types to enum flags")]
    public class AlterPadSeriesPolisherTypesToFlags : Migration {
        public override void Up() {
            Create.Column("polisher_types").OnTable("pad_series").AsInt16().WithDefaultValue(0);

            Execute.WithConnection((c, t) => {
                var polisherTypes = c.Query("select * from pad_series_polisher_types");

                var newPolisherTypes = new List<(Guid PadSeriesId, PolisherType PolisherType)>();

                foreach (var pt in polisherTypes) {
                    newPolisherTypes.Add((PadSeriesId: (Guid)pt.pad_series_id, PolisherType: (pt switch {
                        "dual_action" => PolisherType.DualAction,
                        "long_throw" => PolisherType.LongThrow,
                        "forced_rotation" => PolisherType.ForcedRotation,
                        "rotary" => PolisherType.Rotary,
                        "mini" => PolisherType.Mini,
                        _ => PolisherType.None
                    })));
                }

                c.Execute(@"update pad_series set polisher_types = @PolisherTypes where id = @Id;", newPolisherTypes);
            });

            Delete.Table("pad_series_polisher_types");
        }

        public override void Down() {
            Create.Table("pad_polisher_types")
            .WithColumn("pad_id").AsGuid().ForeignKey("pads", "id")
            .WithColumn("polisher_type").AsString(32);

            Execute.WithConnection((c, t) => {
                var polisherTypes = c.Query<(Guid Id, PolisherType PolisherType)>("select id, polisher_types from pad_series");

                var newPolisherTypes = new List<(Guid PadSeriesId, List<string> PolisherTypes)>();

                foreach (var pt in polisherTypes) {
                    List<string> types = new();

                    if (pt.PolisherType.HasFlag(PolisherType.DualAction)) {
                        types.Add("dual_action");
                    }

                    if (pt.PolisherType.HasFlag(PolisherType.LongThrow)) {
                        types.Add("dual_action");
                    }

                    if (pt.PolisherType.HasFlag(PolisherType.ForcedRotation)) {
                        types.Add("forced_rotation");
                    }

                    if (pt.PolisherType.HasFlag(PolisherType.Mini)) {
                        types.Add("mini");
                    }

                    if (pt.PolisherType.HasFlag(PolisherType.Rotary)) {
                        types.Add("rotary");
                    }

                    newPolisherTypes.Add((PadSeriesId: pt.Id, PolisherTypes: types));
                }

                foreach (var newPT in newPolisherTypes) {
                    foreach (var pt in newPT.PolisherTypes) {
                        c.Execute(@"insert into pad_series_polisher_types (pad_series_id, polisher_type) values (@PadSeriesId, @PolisherType);", new {
                            PadSeriesId = newPT.PadSeriesId,
                            PolisherType = pt
                        });

                    }
                }
            });

            Delete.Column("polisher_types").FromTable("pad_series_polisher_types");
        }
    }
}