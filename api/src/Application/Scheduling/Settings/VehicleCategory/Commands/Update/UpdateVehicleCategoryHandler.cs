using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Validation(typeof(UpdateVehicleCategoryValidator))]
    [Authorization(Action = "update", Scope = "vehicle-categories")]
    public class UpdateVehicleCategoryHandler : ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryView> {
        private IVehicleCategoryService service;
        private IMapper mapper;

        public UpdateVehicleCategoryHandler(IVehicleCategoryService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
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

            return mapper.Map<VehicleCategory, VehicleCategoryView>(cat);
        }
    }
}