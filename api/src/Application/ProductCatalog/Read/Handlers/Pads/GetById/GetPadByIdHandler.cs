using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetPadByIdHandler : ActionHandler<GetPadByIdQuery, PadSummaryReadModel?> {
        IPadSummaryReader reader;
        public GetPadByIdHandler(IPadSummaryReader reader) {
            this.reader = reader;
        }

        public async override Task<PadSummaryReadModel?> Execute(GetPadByIdQuery input, User? user = null) {
            var p = await reader.Read(input.Id);
            return p;
        }
    }
}