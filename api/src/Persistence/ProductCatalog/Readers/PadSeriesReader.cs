using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesReader : DatabaseInteractor, IPadSeriesReader {
        public PadSeriesReader(IDatabase database) : base(database) { }

        public async Task<PadSeriesReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"  select * from pad_series ps join brands b on ps.brand_id = b.id where ps.id = @Id;
                        select * from pad_series_polisher_types where pad_series_id = @Id;
                        select * from pad_sizes where pad_series_id = @Id;
                        select count(reviews.*) as count, pads.id from pads
                            left join reviews on reviews.pad_id = pads.id 
                            where pad_series_id = @Id
                            group by pads.id;
                        select pi.* from pad_images pi join pads p on pi.pad_id = p.id where p.pad_series_id = @Id;
                        select pc.*, avg(r.cut) as cut, avg(r.finish) as finish, coalesce(avg(r.stars), 0) as stars from pads pc 
                            left join reviews r on pc.id = r.pad_id 
                            where pad_series_id = @Id group by pc.id;
                        select * from pad_options po left join pads pc on po.pad_id = pc.id where pad_series_id = @Id;
                        select po.id as pad_option_id, pn.* from part_numbers pn join pad_option_part_numbers popn on pn.id = popn.part_number_id join pad_options po on po.id = popn.pad_option_id join pads p on po.pad_id = p.id where p.pad_series_id = @Id; 
                    ",
                    new { Id = id }
                )) {
                    var series = reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                        (ps, b) => new PadSeriesReadModel(
                            ps.Id,
                            ps.Name,
                            new BrandReadModel(
                                b.Id,
                                b.Name
                            )
                        )).ElementAt(0);

                    series.PolisherTypes.AddRange(reader.Read<PadSeriesPolisherTypeRow>()
                        .Select(p => p.PolisherType));

                    series.Sizes.AddRange(reader.Read<PadSizeRow>()
                        .Select(s => new PadSizeReadModel(
                            s.Id,
                            new MeasurementReadModel(s.DiameterAmount, s.DiameterUnit),
                            s.ThicknessAmount != null ? new MeasurementReadModel(s.ThicknessAmount ?? 0, s.ThicknessUnit!) : null
                            )));

                    var reviewCount = reader.ReadFirstOrDefault<int>();
                    var images = reader.Read<(Guid PadId, Guid ImageId)>();

                    var keyValues = new List<KeyValuePair<Guid, PadReadModel>>();
                    foreach (var pad in reader.Read()) {
                        Guid? imageId = images.Where(i => i.PadId == pad.id).FirstOrDefault().ImageId;

                        if (imageId == Guid.Empty) {
                            imageId = null;
                        }

                        keyValues.Add(new KeyValuePair<Guid, PadReadModel>(
                            pad.id,
                            new PadReadModel(
                                pad.id,
                                pad.name,
                                pad.category,
                                pad.material,
                                pad.texture,
                                pad.color,
                                pad.has_center_hole,
                                imageId,
                                new List<PadOptionReadModel>(),
                                pad.cut,
                                pad.finish,
                                new RatingReadModel(pad.stars, reviewCount)
                            )
                        ));
                    }

                    var pads = new Dictionary<Guid, PadReadModel>(keyValues);

                    var options = reader.Read<PadOptionRow>();
                    var optionDict = new Dictionary<Guid, PadOptionReadModel>();
                    foreach (var opt in options) {
                        PadReadModel? pad;

                        if (pads.TryGetValue(opt.PadId, out pad)) {
                            var po = new PadOptionReadModel(opt.PadSizeId);
                            pad.Options.Add(po);
                            optionDict.Add(opt.Id, po);
                        }
                    }

                    var partNumbers = reader.Read<(Guid PadOptionId, PartNumberRow PartNumber)>();
                    foreach (var partNumber in partNumbers) {
                        PadOptionReadModel? option;

                        if (optionDict.TryGetValue(partNumber.PadOptionId, out option)) {
                            option.PartNumbers.Add(new PartNumberReadModel(partNumber.PartNumber.Value, partNumber.PartNumber.Notes));
                        }
                    }

                    series.Pads.AddRange(pads.Values);
                    return series;
                }
            }
        }

        public async Task<List<PadSeriesReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"  select * from pad_series ps join brands b on ps.brand_id = b.id;
                        select * from pad_series_polisher_types;
                        select * from pad_sizes;
                        select count(reviews.*) as count, pads.id from pads
                            left join reviews on reviews.pad_id = pads.id 
                            group by pads.id;
                        select pi.* from pad_images pi join pads p on pi.pad_id = p.id;
                        select pc.*, avg(r.cut) as cut, avg(r.finish) as finish, coalesce(avg(r.stars), 0) as stars from pads pc 
                            left join reviews r on pc.id = r.pad_id 
                            group by pc.id;
                        select * from pad_options po left join pads pc on po.pad_id = pc.id;
                        select po.id as pad_option_id, pn.* from part_numbers pn join pad_option_part_numbers popn on pn.id = popn.part_number_id join pad_options po on po.id = popn.pad_option_id; 
                        "
                )) {
                    var series = new Dictionary<Guid, PadSeriesReadModel>(
                        reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                            (ps, b) => new PadSeriesReadModel(
                                ps.Id,
                                ps.Name,
                                new BrandReadModel(
                                    b.Id,
                                    b.Name
                                )
                            )
                        ).Select(p => new KeyValuePair<Guid, PadSeriesReadModel>(p.Id, p))
                    );

                    var polisherTypes = reader.Read<PadSeriesPolisherTypeRow>();
                    foreach (var pt in polisherTypes) {
                        PadSeriesReadModel? s;

                        if (series.TryGetValue(pt.PadSeriesId, out s)) {
                            s.PolisherTypes.Add(pt.PolisherType);
                        }
                    }

                    var sizes = reader.Read<PadSizeRow>();
                    foreach (var size in sizes) {
                        PadSeriesReadModel? s;

                        if (series.TryGetValue(size.PadSeriesId, out s)) {
                            s.Sizes.Add(new PadSizeReadModel(
                                size.Id,
                                new MeasurementReadModel(size.DiameterAmount, size.DiameterUnit),
                                size.ThicknessAmount != null ? new MeasurementReadModel(size.ThicknessAmount ?? 0, size.ThicknessUnit!) : null
                            )

                            );
                        }
                    }

                    var reviewCounts = new Dictionary<Guid, int>(reader.Read<(int Count, Guid Id)>().Select(c => new KeyValuePair<Guid, int>(c.Id, c.Count)));
                    var images = reader.Read<(Guid PadId, Guid ImageId)>();

                    var pads = new Dictionary<Guid, PadReadModel>();

                    foreach (var raw in reader.Read()) {
                        Guid? imageId = images.Where(i => i.PadId == raw.id).FirstOrDefault().ImageId;

                        if (imageId == Guid.Empty) {
                            imageId = null;
                        }

                        Guid id = raw.id;
                        string name = raw.name;
                        string category = raw.category;
                        decimal? cut = raw.cut;
                        decimal? finish = raw.finish;

                        var pad = new PadReadModel(
                            raw.id,
                            raw.name,
                            raw.category,
                            raw.material,
                            raw.texture,
                            raw.color,
                            raw.has_center_hole,
                            imageId,
                            new List<PadOptionReadModel>(),
                            raw.cut,
                            raw.finish,
                            new RatingReadModel(raw.stars, reviewCounts[raw.id])
                        );


                        pads.Add(pad.Id, pad);

                        PadSeriesReadModel? s;

                        if (series.TryGetValue(raw.pad_series_id, out s)) {
                            s!.Pads.Add(pad);
                        }
                    }

                    var options = reader.Read<PadOptionRow>();
                    var optionDict = new Dictionary<Guid, PadOptionReadModel>();

                    foreach (var opt in options) {
                        PadReadModel? pad;

                        if (pads.TryGetValue(opt.PadId, out pad)) {
                            var po = new PadOptionReadModel(opt.PadSizeId);
                            pad.Options.Add(po);
                            optionDict.Add(opt.Id, po);
                        }
                    }

                    var partNumbers = reader.Read<(Guid PadOptionId, PartNumberRow PartNumber)>();
                    foreach (var partNumber in partNumbers) {
                        PadOptionReadModel? option;

                        if (optionDict.TryGetValue(partNumber.PadOptionId, out option)) {
                            option.PartNumbers.Add(new PartNumberReadModel(partNumber.PartNumber.Value, partNumber.PartNumber.Notes));
                        }
                    }

                    return series.Values.ToList();
                }
            }
        }
    }
}