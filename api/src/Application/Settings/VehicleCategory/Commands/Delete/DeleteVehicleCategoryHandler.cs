using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validation(typeof(DeleteVehicleCategoryValidator))]
    public class DeleteVehicleCategoryHandler : ActionHandler<DeleteVehicleCategoryCommand> {
        private VehicleCategoryNotInuseSpecification specification;
        private IVehicleCategoryRepo repo;

        public DeleteVehicleCategoryHandler(VehicleCategoryNotInuseSpecification specification, IVehicleCategoryRepo repo) {
            this.specification = specification;
            this.repo = repo;
        }

        public async override Task Execute(DeleteVehicleCategoryCommand command, User? user) {
            var cat = await repo.FindById(command.Id);

            if (cat == null) {
                throw new EntityNotFoundException();
            }

            if (cat.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            await specification.CheckAndThrow(cat);

            await repo.Delete(cat);
        }
    }
}