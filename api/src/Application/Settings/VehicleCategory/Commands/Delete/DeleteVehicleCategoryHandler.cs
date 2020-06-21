using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Validation(typeof(DeleteVehicleCategoryValidator))]
    [Authorization(Action = "delete", Scope = "vehicle-categories")]
    public class DeleteVehicleCategoryHandler : ActionHandler<DeleteVehicleCategoryCommand> {
        IVehicleCategoryService service;

        public DeleteVehicleCategoryHandler(IVehicleCategoryService service) {
            this.service = service;
        }

        public async override Task Execute(DeleteVehicleCategoryCommand command, User? user) {
            var cat = await service.FindById(command.Id);

            if (cat == null) {
                throw new EntityNotFoundException();
            }

            if (!cat.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Delete(cat);
        }
    }
}