using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "vehicle-categories")]
    [DependencyInjection]
    public class GetVehicleCategoriesHandler : ActionHandler<GetVehicleCategoriesQuery, List<VehicleCategoryView>> {
        private IVehicleCategoryService service;

        public GetVehicleCategoriesHandler(IVehicleCategoryService service) {
            this.service = service;
        }

        public async override Task<List<VehicleCategoryView>> Execute(GetVehicleCategoriesQuery query, User? user) {
            var vcs = await service.GetByUser(user!);
            throw new NotImplementedException();
            // return mapper.Map<List<VehicleCategory>, List<VehicleCategoryView>>(vcs);
        }
    }
}