using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validate(typeof(UpdateVehicleCategoryValidator))]
    public class UpdateVehicleCategoryHandler : ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryDto> {
        private VehicleCategoryNameUniqueSpecification specifcation;
        private IVehicleCategoryRepo repo;
        private IMapper mapper;

        public UpdateVehicleCategoryHandler(VehicleCategoryNameUniqueSpecification specifcation, IVehicleCategoryRepo repo, IMapper mapper) {
            this.specifcation = specifcation;
            this.repo = repo;
            this.mapper = mapper;
        }

        protected async override Task<VehicleCategoryDto> Execute(UpdateVehicleCategoryCommand command, User? user) {
            var cat = await repo.FindById(command.Id);

            if (cat == null) {
                throw new EntityNotFoundException();
            }

            if (cat.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            cat.Name = command.Name;
            cat.Description = command.Description;

            // Ensure new name isn't in use by another category.
            await specifcation.CheckAndThrow(cat);

            await repo.Update(cat);
            return mapper.Map<VehicleCategory, VehicleCategoryDto>(cat);
        }
    }
}