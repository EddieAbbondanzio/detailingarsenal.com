using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "businesses")]
    public class GetBusinessHandler : ActionHandler<GetBusinessQuery, BusinessView> {
        private IBusinessService service;

        public GetBusinessHandler(IBusinessService service) {
            this.service = service;
        }

        public async override Task<BusinessView> Execute(GetBusinessQuery query, User? user) {
            var b = await service.GetOrCreateForUser(user!);
            throw new NotImplementedException();
            // return mapper.Map<Business, BusinessView>(b);
        }
    }
}