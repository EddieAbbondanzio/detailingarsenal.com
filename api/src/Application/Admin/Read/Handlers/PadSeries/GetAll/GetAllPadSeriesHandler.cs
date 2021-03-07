using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [DependencyInjection]
    [Authorization(Scope = "pad-series", Action = "read")]
    public class GetAllPadSeriesHandler : ActionHandler<GetAllPadSeriesQuery, PagedCollection<PadSeriesReadModel>> {
        IPadSeriesReader reader;

        public GetAllPadSeriesHandler(IPadSeriesReader reader) {
            this.reader = reader;
        }

        public async override Task<PagedCollection<PadSeriesReadModel>> Execute(GetAllPadSeriesQuery input, User? user) {
            var series = await reader.ReadAll(input.Paging);
            return series;
        }
    }
}