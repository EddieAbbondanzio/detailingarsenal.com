using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "create", Scope = "vehicle-categories")]
    [Validation(typeof(CreateVehicleCategoryValidator))]
    [DependencyInjection]
    public class CreateVehicleCategoryHandler : ActionHandler<CreateVehicleCategoryCommand, VehicleCategoryView> {
        private IVehicleCategoryService service;

        public CreateVehicleCategoryHandler(IVehicleCategoryService service) {
            this.service = service;
        }

        public async override Task<VehicleCategoryView> Execute(CreateVehicleCategoryCommand command, User? user) {
            var cat = await service.Create(
                new VehicleCategoryCreate(
                    command.Name,
                    command.Description
                ),
                user!
            );

            throw new NotImplementedException();
            // return mapper.Map<VehicleCategory, VehicleCategoryView>(cat);
        }
    }
}