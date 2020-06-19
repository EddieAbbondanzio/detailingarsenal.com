using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application {
    [Validation(typeof(DeleteVehicleCategoryValidator))]
    [Authorization(Action = "delete", Scope = "vehicle-categories")]
    public class DeleteVehicleCategoryHandler : ActionHandler<DeleteVehicleCategoryCommand> {
        IVehicleCategoryService service;

        public DeleteVehicleCategoryHandler(IVehicleCategoryService service) {
            this.service = service;
        }

        public async override Task Execute(DeleteVehicleCategoryCommand command, User? user) {
            var cat = await repo.FindById(command.Id);

            if (cat == null) {
                throw new EntityNotFoundException();
            }

            if (cat.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            await service.Delete(cat, user!);
        }
    }
}