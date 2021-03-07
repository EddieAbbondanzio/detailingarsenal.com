using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "services")]
    [DependencyInjection]
    public class GetServicesHandler : ActionHandler<GetServicesQuery, List<ServiceView>> {
        private IServiceService service;

        public GetServicesHandler(IServiceService service) {
            this.service = service;
        }

        public async override Task<List<ServiceView>> Execute(GetServicesQuery input, User? user) {
            var services = await service.GetByUser(user!);
            throw new NotImplementedException();
            // return mapper.Map<List<Service>, List<ServiceView>>(services);
        }
    }
}