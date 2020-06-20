using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "vehicle-categories")]
    public class GetVehicleCategoriesHandler : ActionHandler<GetVehicleCategoriesQuery, List<VehicleCategoryDto>> {
        private IVehicleCategoryRepo repo;
        private IMapper mapper;

        public GetVehicleCategoriesHandler(IVehicleCategoryRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<List<VehicleCategoryDto>> Execute(GetVehicleCategoriesQuery query, User? user) {
            var vcs = await repo.FindByUser(user!);
            return mapper.Map<List<VehicleCategory>, List<VehicleCategoryDto>>(vcs);
        }
    }
}