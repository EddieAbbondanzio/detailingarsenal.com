using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetAllPadsHandler : ActionHandler<GetAllPadsQuery, PagedCollection<PadSummaryReadModel>> {
        IPadSummaryReader reader;

        public GetAllPadsHandler(IPadSummaryReader reader) {
            this.reader = reader;
        }

        public async override Task<PagedCollection<PadSummaryReadModel>> Execute(GetAllPadsQuery input, User? user) {
            var pads = await reader.ReadAll();
            return pads;                
        }
    }
}