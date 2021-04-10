using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [DependencyInjection]
    public class GetFilterHandler : ActionHandler<GetPadFilterQuery, PadFilterReadModel> {
        IPadFilterReader reader;

        public GetFilterHandler(IPadFilterReader reader) {
            this.reader = reader;
        }

        public async override Task<PadFilterReadModel> Execute(GetPadFilterQuery input, User? user) {
            var f = await reader.Read();
            return f;
        }
    }
}