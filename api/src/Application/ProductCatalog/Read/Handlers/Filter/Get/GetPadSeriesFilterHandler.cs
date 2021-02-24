using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetPadSeriesFilterHandler : ActionHandler<GetPadSeriesFilterQuery, PadSeriesFilterReadModel> {
        IPadSeriesFilterReader reader;

        public GetPadSeriesFilterHandler(IPadSeriesFilterReader reader) {
            this.reader = reader;
        }

        public async override Task<PadSeriesFilterReadModel> Execute(GetPadSeriesFilterQuery input, User? user) {
            var f = await reader.Read();
            return f;
        }
    }
}