using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetAllPadSeriesHandler : ActionHandler<GetAllPadSeriesQuery, List<PadSeriesReadModel>> {
        IPadSeriesReader reader;

        public GetAllPadSeriesHandler(IPadSeriesReader reader) {
            this.reader = reader;
        }

        public async override Task<List<PadSeriesReadModel>> Execute(GetAllPadSeriesQuery input, User? user) {
            var series = await reader.ReadAll();
            return series;
        }
    }
}