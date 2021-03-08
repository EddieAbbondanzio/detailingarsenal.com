using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(IPadSizeReader))]
    public class PadSizeReader : DatabaseInteractor, IPadSizeReader {
        public PadSizeReader(IDatabase database) : base(database) { }

        public async Task<List<PadSizeReadModel>> ReadSizesForPad(Guid padId) {
            using (var conn = OpenConnection()) {
                var sizes = await conn.QueryAsync(@"
                        select ps.* from pad_options po
                        left join pad_sizes ps on po.pad_size_id = ps.id
                        where po.pad_id = @Id;
                        ", new { Id = padId }
                );

                return sizes.Select(s => new PadSizeReadModel(
                    new MeasurementReadModel(s.diameter_amount, s.diameter_unit),
                    s.thickness_amount != null ? new MeasurementReadModel(s.thickness_amount, s.thickness_unit) : null
                )).ToList();
            }
        }
    }
}