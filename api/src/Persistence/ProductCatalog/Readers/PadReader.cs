using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Persistence.Shared;

namespace DetailingArsenal.Persistence.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(IPadReader))]
    public class PadReader : DatabaseInteractor, IPadReader {
        public PadReader(IDatabase database) : base(database) { }

        public async Task<PadReadModel?> Read(Guid id) {
            using (var conn = OpenConnection()) {
                var r = await conn.QueryFirstOrDefaultAsync<PadsViewRow>(@"
                    select * from pads_view where id = @Id;
                ", new { Id = id });

                return Map(r);
            }
        }

        public async Task<PagedCollection<PadReadModel>> ReadFiltered(GetPadsFilteredQuery query) {
            var queryBuilder = new PadsViewQueryBuilder();

            if (query.Brands.Count > 0) {
                queryBuilder.AddBrandFilter();
            }

            if (query.Series.Count > 0) {
                queryBuilder.AddSeriesFilter();
            }

            if (query.Categories.Count > 0) {
                queryBuilder.AddCategoriesFilter();
            }

            if (query.Materials.Count > 0) {
                queryBuilder.AddMaterialsFilter();
            }

            if (query.Textures.Count > 0) {
                queryBuilder.AddTexturesFilter();
            }

            if (query.PolisherTypes.Count > 0) {
                queryBuilder.AddPolisherTypesFilter();
            }

            if (query.HasCenterHole.Count > 0) {
                queryBuilder.AddHasCenterHoleFilter();
            }

            if (query.Stars.Count > 0) {
                queryBuilder.AddStarsFilter();
            }

            using (var conn = OpenConnection()) {
                var pads = await conn.QueryAsync<PadsViewRow>(
                    queryBuilder.ToString(),
                    new {
                        Brands = query.Brands,
                        Series = query.Series,
                        Categories = query.Categories.Flatten(),
                        Materials = query.Materials,
                        Textures = query.Textures,
                        PolisherTypes = query.PolisherTypes.Flatten(),
                        HasCenterHole = query.HasCenterHole,
                        Stars = query.Stars
                    }
                );

                return new PagedCollection<PadReadModel>(new Paging(query)) pads.Select(Map).ToList();
            }
        }

        public async Task<PagedCollection<PadReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                var raws = await conn.QueryAsync<PadsViewRow>(@"
                    select * from pads_view;
                ");

                var summaries = raws.Select(Map);

                return new PagedCollection<PadReadModel>(new Paging(0, 20, summaries.Count()), summaries.ToArray());
            }
        }

        PadReadModel Map(PadsViewRow row) => new PadReadModel(
            row.Id,
            row.Name,
            new PadSeriesReadModel(row.PadSeriesId, row.PadSeriesName),
            new PadBrandReadModel(row.BrandId, row.BrandName),
            ((PadCategoryBitwise)row.Category).ToList(),
            PadMaterialUtils.Parse(row.Material),
            PadTextureUtils.Parse(row.Texture),
            row.HasCenterHole,
            ((PolisherTypeBitwise)row.PolisherTypes).ToList(),
            row.Cut,
            row.Finish,
            new RatingReadModel(row.Stars, row.ReviewCount)
        );

    }
}