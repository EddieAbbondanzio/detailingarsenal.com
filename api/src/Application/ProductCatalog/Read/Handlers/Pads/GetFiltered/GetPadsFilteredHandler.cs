using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [DependencyInjection]
    public class GetPadsFilteredHandler : ActionHandler<GetPadsFilteredQuery, PagedCollection<PadReadModel>> {
        IPadReader reader;

        public GetPadsFilteredHandler(IPadReader reader) {
            this.reader = reader;
        }

        public async override Task<PagedCollection<PadReadModel>> Execute(GetPadsFilteredQuery input, User? user) {
            var pads = await reader.ReadFiltered(input);
            return pads;
        }
    }
}