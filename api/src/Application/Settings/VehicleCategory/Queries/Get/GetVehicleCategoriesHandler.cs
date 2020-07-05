using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "vehicle-categories")]
    public class GetVehicleCategoriesHandler : ActionHandler<GetVehicleCategoriesQuery, List<VehicleCategoryView>> {
        private IVehicleCategoryService service;
        private IMapper mapper;

        public GetVehicleCategoriesHandler(IVehicleCategoryService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<VehicleCategoryView>> Execute(GetVehicleCategoriesQuery query, User? user) {
            var vcs = await service.GetByUser(user!);
            return mapper.Map<List<VehicleCategory>, List<VehicleCategoryView>>(vcs);
        }
    }
}