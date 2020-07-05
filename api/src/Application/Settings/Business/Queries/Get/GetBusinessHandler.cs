using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "businesses")]
    public class GetBusinessHandler : ActionHandler<GetBusinessQuery, BusinessView> {
        private IBusinessService service;
        private IMapper mapper;

        public GetBusinessHandler(IBusinessService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<BusinessView> Execute(GetBusinessQuery query, User? user) {
            var b = await service.GetOrCreateForUser(user!);
            return mapper.Map<Business, BusinessView>(b);
        }
    }
}