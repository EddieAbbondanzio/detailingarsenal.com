using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validation(typeof(UpdateVehicleCategoryValidator))]
    public class UpdateVehicleCategoryHandler : ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryDto> {
        private VehicleCategoryNameUniqueSpecification specification;
        private IVehicleCategoryRepo repo;
        private IMapper mapper;

        public UpdateVehicleCategoryHandler(VehicleCategoryNameUniqueSpecification specification, IVehicleCategoryRepo repo, IMapper mapper) {
            this.specification = specification;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<VehicleCategoryDto> Execute(UpdateVehicleCategoryCommand command, User? user) {
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
            await specification.CheckAndThrow(cat);

            await repo.Update(cat);
            return mapper.Map<VehicleCategory, VehicleCategoryDto>(cat);
        }
    }
}