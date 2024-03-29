using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [DependencyInjection]
    [Authorization(Scope = "pad-series", Action = "read")]
    public class GetPadSeriesByIdHandler : ActionHandler<GetPadSeriesByIdQuery, PadSeriesReadModel?> {
        IPadSeriesReader reader;

        public GetPadSeriesByIdHandler(IPadSeriesReader reader) {
            this.reader = reader;
        }

        public async override Task<PadSeriesReadModel?> Execute(GetPadSeriesByIdQuery input, User? user) {
            return await reader.ReadById(input.Id);
        }
    }
}