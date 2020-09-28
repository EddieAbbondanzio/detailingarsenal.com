using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesRepo : DatabaseInteractor, IPadSeriesRepo {
        public PadSeriesRepo(IDatabase database) : base(database) { }

        public async Task<PadSeries?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select * from pad_series where id = @Id;
                
                select * from pads where pad_series_id = @Id;",
                    new {
                        Id = id
                    }
                )) {
                    var seriesModel = reader.ReadFirstOrDefault<PadSeriesRow>();

                    if (seriesModel == null) {
                        return null;
                    }

                    throw new NotImplementedException();

                    // var padModels = reader.Read<PadRow>();

                    // var pads = padModels.Select(p => new Pad(
                    //     p.Id, PadCategoryUtils.Parse(p.Category), p.Name, p.ImageName != null ? new DataUrlImage(p.ImageName, p.ImageData!) : null
                    // )).ToList();

                    // return new PadSeries(seriesModel.Id, seriesModel.Name, seriesModel.BrandId, pads);
                }
            }
        }

        public async Task Add(PadSeries entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"insert into pad_series (id, brand_id, name) values (@Id, @BrandId, @Name);", entity
                    );

                    var pads = entity.Pads.Select(p => new PadRow() {
                        Id = p.Id,
                        Category = p.Category.Serialize(),
                        Name = p.Name,
                        PadSeriesId = entity.Id,
                        ImageName = p.Image?.Name,
                        ImageData = p.Image?.ToBinary()
                    }).ToList();

                    await conn.ExecuteAsync(
                        @"insert into pads (id, pad_series_id, category, name, image_name, image_data) values (@Id, @PadSeriesId, @Category, @Name, @ImageName, @ImageData);",
                        pads
                    );



                    t.Commit();
                }
            }
        }

        public async Task Update(PadSeries entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"update pad_series set brand_id = @BrandId, name = @Name where id = @Id;", entity
                    );

                    await conn.ExecuteAsync(@"delete from pads where pad_series_id = @Id;", entity.Id);

                    var pads = entity.Pads.Select(p => new PadRow() {
                        Id = p.Id,
                        Category = p.Category.Serialize(),
                        Name = p.Name,
                        PadSeriesId = entity.Id,
                        ImageName = p.Image?.Name,
                        ImageData = p.Image?.ToBinary()
                    }).ToList();

                    await conn.ExecuteAsync(
                        @"insert into pads (id, pad_series_id, category, name, image_name, image_data) values (@Id, @PadSeriesId, @Category, @Name, @ImageName, @ImageData);",
                        pads
                    );



                    t.Commit();
                }
            }
        }

        public async Task Delete(PadSeries entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    // Delete out pads first due to foreign key
                    await conn.ExecuteAsync(@"delete from pads where pad_series_id = @Id;", entity);
                    await conn.ExecuteAsync(@"delete from pad_series where id = @Id;", entity);

                    t.Commit();
                }
            }
        }
    }
}