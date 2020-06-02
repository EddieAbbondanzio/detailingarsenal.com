using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization()]
    [Validation(typeof(CreateVehicleCategoryValidator))]
    public class CreateVehicleCategoryHandler : ActionHandler<CreateVehicleCategoryCommand, VehicleCategoryDto> {
        private VehicleCategoryNameUniqueSpecification specifcation;
        private IVehicleCategoryRepo repo;
        private IMapper mapper;

        public CreateVehicleCategoryHandler(VehicleCategoryNameUniqueSpecification specifcation, IVehicleCategoryRepo repo, IMapper mapper) {
            this.specifcation = specifcation;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<VehicleCategoryDto> Execute(CreateVehicleCategoryCommand command, User? user) {
            var cat = new VehicleCategory() {
                Id = Guid.NewGuid(),
                UserId = user!.Id,
                Name = command.Name,
                Description = command.Description
            };

            await specifcation.CheckAndThrow(cat);

            await repo.Add(cat);
            return mapper.Map<VehicleCategory, VehicleCategoryDto>(cat);
        }
    }
}