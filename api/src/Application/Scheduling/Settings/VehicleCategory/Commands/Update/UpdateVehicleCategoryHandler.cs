using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;
using System;

namespace DetailingArsenal.Application.Settings {
    [Validation(typeof(UpdateVehicleCategoryValidator))]
    [Authorization(Action = "update", Scope = "vehicle-categories")]
    [DependencyInjection]
    public class UpdateVehicleCategoryHandler : ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryView> {
        private IVehicleCategoryService service;
        public UpdateVehicleCategoryHandler(IVehicleCategoryService service) {
            this.service = service;
        }

        public async override Task<VehicleCategoryView> Execute(UpdateVehicleCategoryCommand command, User? user) {
            VehicleCategory cat = await service.GetById(command.Id);

            if (!cat.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Update(
                cat,
                new VehicleCategoryUpdate(
                    command.Name,
                    command.Description
                )
            );

            throw new NotImplementedException();
            // return mapper.Map<VehicleCategory, VehicleCategoryView>(cat);
        }
    }
}