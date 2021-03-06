using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Persistence.Shared;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_02_28_00, "Alter pad series polisher types to enum flags")]
    public class AlterPadSeriesPolisherTypesToFlags : Migration {
        public override void Up() {
            Create.Column("polisher_types").OnTable("pad_series").AsInt16().WithDefaultValue(0);

            Execute.WithConnection((c, t) => {
                var raws = c.Query("select * from pad_series_polisher_types");
                var padSeriesPolisherTypeMap = new Dictionary<Guid, List<PolisherType>>();

                foreach (var raw in raws) {
                    List<PolisherType> existing;

                    if (!padSeriesPolisherTypeMap.TryGetValue(raw.pad_series_id, out existing)) {
                        existing = new();
                        padSeriesPolisherTypeMap.Add(raw.pad_series_id, existing);
                    }

                    existing.Add(raw.polisher_type switch {
                        "dual_action" => PolisherType.DualAction,
                        "forced_rotation" => PolisherType.ForcedRotation,
                        "long_throw" => PolisherType.LongThrow,
                        "mini" => PolisherType.Mini,
                        "rotary" => PolisherType.Rotary,
                        _ => PolisherType.None
                    });
                }

                var finals = new List<object>();
                foreach (var kv in padSeriesPolisherTypeMap) {
                    finals.Add(new { Id = kv.Key, PolisherType = Flatten(kv.Value) });
                }

                c.Execute(@"update pad_series set polisher_types = @PolisherType where id = @Id;", finals);
            });

            Delete.Table("pad_series_polisher_types");
        }

        public override void Down() {
            Create.Table("pad_series_polisher_types")
            .WithColumn("pad_series_id").AsGuid().ForeignKey("pad_series", "id")
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
                        types.Add("long_throw");
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

        PolisherTypeBitwise Flatten(List<PolisherType> polisherTypes) {
            var bitwise = PolisherTypeBitwise.None;

            for (int i = 0; i < polisherTypes.Count; i++) {
                switch (polisherTypes[i]) {
                    case PolisherType.DualAction:
                        bitwise |= PolisherTypeBitwise.DualAction;
                        break;
                    case PolisherType.LongThrow:
                        bitwise |= PolisherTypeBitwise.LongThrow;
                        break;
                    case PolisherType.ForcedRotation:
                        bitwise |= PolisherTypeBitwise.ForcedRotation;
                        break;
                    case PolisherType.Mini:
                        bitwise |= PolisherTypeBitwise.Mini;
                        break;
                    case PolisherType.Rotary:
                        bitwise |= PolisherTypeBitwise.Rotary;
                        break;
                }
            }

            return bitwise;
        }
    }
}