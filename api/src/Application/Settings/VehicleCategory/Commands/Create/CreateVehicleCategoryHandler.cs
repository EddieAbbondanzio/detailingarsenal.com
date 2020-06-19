using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application {
    [Authorization(Action = "create", Scope = "vehicle-categories")]
    [Validation(typeof(CreateVehicleCategoryValidator))]
    public class CreateVehicleCategoryHandler : ActionHandler<CreateVehicleCategoryCommand, VehicleCategoryDto> {
        private IVehicleCategoryService service;
        private IMapper mapper;

        public CreateVehicleCategoryHandler(IVehicleCategoryService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<VehicleCategoryDto> Execute(CreateVehicleCategoryCommand command, User? user) {
            var cat = await service.Create(
                new CreateVehicleCategory() {
                    Name = command.Name,
                    Description = command.Description
                },
                user!
            );

            return mapper.Map<VehicleCategory, VehicleCategoryDto>(cat);
        }
    }
}