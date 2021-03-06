using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetAllPadsHandler : ActionHandler<GetAllPadsQuery, PagedCollection<PadReadModel>> {
        IPadReader reader;

        public GetAllPadsHandler(IPadReader reader) {
            this.reader = reader;
        }

        public async override Task<PagedCollection<PadReadModel>> Execute(GetAllPadsQuery input, User? user) {
            var pads = await reader.ReadAll();
            return pads;
        }
    }
}