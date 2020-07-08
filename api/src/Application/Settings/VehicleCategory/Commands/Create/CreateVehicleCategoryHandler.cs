using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "create", Scope = "vehicle-categories")]
    [Validation(typeof(CreateVehicleCategoryValidator))]
    public class CreateVehicleCategoryHandler : ActionHandler<CreateVehicleCategoryCommand, VehicleCategoryView> {
        private IVehicleCategoryService service;
        private IMapper mapper;

        public CreateVehicleCategoryHandler(IVehicleCategoryService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<VehicleCategoryView> Execute(CreateVehicleCategoryCommand command, User? user) {
            var cat = await service.Create(
                new VehicleCategoryCreate(
                    command.Name,
                    command.Description
                ),
                user!
            );

            return mapper.Map<VehicleCategory, VehicleCategoryView>(cat);
        }
    }
}