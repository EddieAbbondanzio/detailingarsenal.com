using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [DependencyInjection]
    public class GetPadSeriesFilterHandler : ActionHandler<GetPadSeriesFilterQuery, PadFilterReadModel> {
        IPadFilterReader reader;

        public GetPadSeriesFilterHandler(IPadFilterReader reader) {
            this.reader = reader;
        }

        public async override Task<PadFilterReadModel> Execute(GetPadSeriesFilterQuery input, User? user) {
            var f = await reader.Read();
            return f;
        }
    }
}