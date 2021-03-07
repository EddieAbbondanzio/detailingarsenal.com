using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;
using DetailingArsenal;

namespace DetailingArsenal.Application.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(ActionHandler<GetPadByIdQuery, PadReadModel?>))]
    public class GetPadByIdHandler : ActionHandler<GetPadByIdQuery, PadReadModel?> {
        IPadReader reader;
        public GetPadByIdHandler(IPadReader reader) {
            this.reader = reader;
        }

        public async override Task<PadReadModel?> Execute(GetPadByIdQuery input, User? user = null) {
            var p = await reader.Read(input.Id);
            return p;
        }
    }
}