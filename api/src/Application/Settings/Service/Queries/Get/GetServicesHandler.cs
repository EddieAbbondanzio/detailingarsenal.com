using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "read", Scope = "services")]
    public class GetServicesHandler : ActionHandler<GetServicesQuery, List<ServiceDto>> {
        private IServiceRepo repo;
        private IMapper mapper;

        public GetServicesHandler(IServiceRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<List<ServiceDto>> Execute(GetServicesQuery input, User? user) {
            var services = await repo.FindByUser(user!);
            return mapper.Map<List<Service>, List<ServiceDto>>(services);
        }
    }
}