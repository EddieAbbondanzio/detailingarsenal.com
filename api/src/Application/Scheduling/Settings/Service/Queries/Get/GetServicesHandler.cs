using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "services")]
    public class GetServicesHandler : ActionHandler<GetServicesQuery, List<ServiceView>> {
        private IServiceService service;
        private IMapper mapper;

        public GetServicesHandler(IServiceService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<ServiceView>> Execute(GetServicesQuery input, User? user) {
            var services = await service.GetByUser(user!);
            return mapper.Map<List<Service>, List<ServiceView>>(services);
        }
    }
}