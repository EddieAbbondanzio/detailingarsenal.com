using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [DependencyInjection]
    public class GetAllSizesForPadHandler : ActionHandler<GetAllSizesForPadQuery, List<PadSizeReadModel>> {
        IPadSizeReader reader;

        public GetAllSizesForPadHandler(IPadSizeReader reader) {
            this.reader = reader;
        }

        public async override Task<List<PadSizeReadModel>> Execute(GetAllSizesForPadQuery input, User? user = null) {
            var sizes = await reader.ReadSizesForPad(input.PadId);
            return sizes;
        }
    }
}