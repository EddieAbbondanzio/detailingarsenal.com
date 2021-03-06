using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
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