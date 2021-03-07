using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using System.Text;
using DetailingArsenal.Persistence.Shared;
using DetailingArsenal.Application.Admin.ProductCatalog;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(IPadSeriesReader))]
    public class PadSeriesReader : DatabaseInteractor, IPadSeriesReader {
        public PadSeriesReader(IDatabase database) : base(database) { }

        public async Task<PadSeriesReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"  select * from pad_series ps 
                            join brands b on ps.brand_id = b.id 
                            where ps.id = @Id;
                        select * from pad_sizes 
                            where pad_series_id = @Id;
                        select pi.* from pad_images pi 
                            join pads p on pi.pad_id = p.id 
                            where p.pad_series_id = @Id;
                        select p.* from pads p 
                            where pad_series_id = @Id group by pc.id;
                        select po.* from pad_options po 
                            left join pads pc on po.pad_id = pc.id 
                            where pad_series_id = @Id;
                        select po.id as pad_option_id, pn.* from part_numbers pn 
                            join pad_option_part_numbers popn on pn.id = popn.part_number_id 
                            join pad_options po on po.id = popn.pad_option_id 
                            join pads p on po.pad_id = p.id 
                            where p.pad_series_id = @Id; 
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
                            ),
                            ps.PolisherTypes.ToList()
                        )).ElementAtOrDefault(0);

                    if (series == null) {
                        return null;
                    }


                    series.Sizes.AddRange(reader.Read<PadSizeRow>()
                        .Select(s => new PadSizeReadModel(
                            s.Id,
                            new MeasurementReadModel(s.DiameterAmount, s.DiameterUnit),
                            s.ThicknessAmount != null ? new MeasurementReadModel(s.ThicknessAmount ?? 0, s.ThicknessUnit!) : null
                            )));

                    var images = reader.Read<(Guid PadId, Guid ImageId)>();

                    var keyValues = new List<KeyValuePair<Guid, PadReadModel>>();
                    var rawPads = reader.Read();

                    foreach (var pad in rawPads) {
                        Guid? imageId = images.Where(i => i.PadId == pad.id).FirstOrDefault().ImageId;

                        if (imageId == Guid.Empty) {
                            imageId = null;
                        }

                        keyValues.Add(new KeyValuePair<Guid, PadReadModel>(
                            pad.id,
                            new PadReadModel(
                                pad.id,
                                pad.name,
                                ((PadCategoryBitwise)pad.category).ToList(),
                                pad.material,
                                pad.texture,
                                pad.color,
                                pad.has_center_hole,
                                imageId,
                                new List<PadOptionReadModel>()
                            )
                        ));
                    }

                    var pads = new Dictionary<Guid, PadReadModel>(keyValues);

                    var options = reader.Read<PadOptionRow>();
                    var optionDict = new Dictionary<Guid, PadOptionReadModel>();
                    foreach (var opt in options) {
                        PadReadModel? pad;

                        if (pads.TryGetValue(opt.PadId, out pad)) {
                            var po = new PadOptionReadModel(opt.Id, opt.PadSizeId);
                            pad.Options.Add(po);
                            optionDict.Add(opt.Id, po);
                        }
                    }

                    var partNumbers = reader.Read<Guid, PartNumberRow, (Guid PadOptionId, PartNumberRow PartNumber)>((id, pn) => (id, pn));
                    foreach (var partNumber in partNumbers) {
                        PadOptionReadModel? option;

                        if (optionDict.TryGetValue(partNumber.PadOptionId, out option)) {
                            option.PartNumbers.Add(new PartNumberReadModel(partNumber.PartNumber.Id, partNumber.PartNumber.Value, partNumber.PartNumber.Notes));
                        }
                    }

                    series.Pads.AddRange(pads.Values);
                    return series;
                }
            }
        }


        public async Task<PagedCollection<PadSeriesReadModel>> ReadAll(PagingOptions paging) {
            using (var conn = OpenConnection()) {
                // Pull in the parent records first
                var series = await conn.QueryAsync<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                    @"
                    select * from pad_series ps 
                    join brands b on ps.brand_id = b.id
                    order by b.name limit @Limit offset @Offset;
                    ",
                    (ps, b) => new PadSeriesReadModel(
                        ps.Id,
                        ps.Name,
                        new BrandReadModel(
                            b.Id,
                            b.Name
                        ),
                        ps.PolisherTypes.ToList()
                    ),
                    new {
                        Limit = paging.PageSize,
                        Offset = paging.Offset
                    }
                );

                var seriesLookup = new Dictionary<Guid, PadSeriesReadModel>(series.Select(p => new KeyValuePair<Guid, PadSeriesReadModel>(p.Id, p)));

                // Now get the rest
                using (var reader = await conn.QueryMultipleAsync(
                    @"
                    select count(*) from pads p;
                    select * from pad_sizes where pad_series_id = any(@Series);
                    select pi.* from pad_images pi 
                        join pads p on pi.pad_id = p.id 
                        where pad_series_id = any(@Series);
                    select p.* from pads p
                        where pad_series_id = any(@Series)
                        group by p.id
                        order by name;
                    select po.* from pad_options po 
                        left join pads pc on po.pad_id = pc.id;
                    select po.id as pad_option_id, pn.* from part_numbers pn 
                        join pad_option_part_numbers popn on pn.id = popn.part_number_id 
                        join pad_options po on po.id = popn.pad_option_id;
                    "
                    , new {
                        Series = series.Select(s => s.Id).ToArray() // We only want series that we got back.
                    })) {
                    var totalCount = reader.ReadFirst<int>();

                    var sizes = reader.Read<PadSizeRow>();
                    foreach (var size in sizes) {
                        PadSeriesReadModel? s;

                        if (seriesLookup.TryGetValue(size.PadSeriesId, out s)) {
                            s.Sizes.Add(new PadSizeReadModel(
                                size.Id,
                                new MeasurementReadModel(size.DiameterAmount, size.DiameterUnit),
                                size.ThicknessAmount != null ? new MeasurementReadModel(size.ThicknessAmount ?? 0, size.ThicknessUnit!) : null
                            )

                            );
                        }
                    }

                    var images = reader.Read<(Guid PadId, Guid ImageId)>();

                    var pads = new Dictionary<Guid, PadReadModel>();
                    var rawPads = reader.Read();

                    foreach (var raw in rawPads) {
                        Guid? imageId = images.Where(i => i.PadId == raw.id).FirstOrDefault().ImageId;

                        if (imageId == Guid.Empty) {
                            imageId = null;
                        }

                        var pad = new PadReadModel(
                            raw.id,
                            raw.name,
                            ((PadCategoryBitwise)raw.category).ToList(),
                            raw.material,
                            raw.texture,
                            raw.color,
                            raw.has_center_hole,
                            imageId,
                            new List<PadOptionReadModel>()
                        );

                        pads.Add(pad.Id, pad);

                        PadSeriesReadModel? s;

                        if (seriesLookup.TryGetValue(raw.pad_series_id, out s)) {
                            s!.Pads.Add(pad);
                        }
                    }

                    var options = reader.Read<PadOptionRow>();
                    var optionDict = new Dictionary<Guid, PadOptionReadModel>();

                    foreach (var opt in options) {
                        PadReadModel? pad;

                        if (pads.TryGetValue(opt.PadId, out pad)) {
                            var po = new PadOptionReadModel(opt.Id, opt.PadSizeId);
                            pad.Options.Add(po);
                            optionDict.Add(opt.Id, po);
                        }
                    }

                    var partNumbers = reader.Read<Guid, PartNumberRow, (Guid PadOptionId, PartNumberRow PartNumber)>((id, pn) => (id, pn));
                    foreach (var partNumber in partNumbers) {
                        PadOptionReadModel? option;

                        if (optionDict.TryGetValue(partNumber.PadOptionId, out option)) {
                            option.PartNumbers.Add(new PartNumberReadModel(partNumber.PartNumber.Id, partNumber.PartNumber.Value, partNumber.PartNumber.Notes));
                        }
                    }

                    return new PagedCollection<PadSeriesReadModel>(new Paging(paging, totalCount), seriesLookup.Values);
                }
            }
        }
    }
}