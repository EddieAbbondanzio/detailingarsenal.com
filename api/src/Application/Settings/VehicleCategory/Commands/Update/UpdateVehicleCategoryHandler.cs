using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Validation(typeof(UpdateVehicleCategoryValidator))]
    [Authorization(Action = "update", Scope = "vehicle-categories")]
    public class UpdateVehicleCategoryHandler : ActionHandler<UpdateVehicleCategoryCommand, VehicleCategoryDto> {
        private IVehicleCategoryService service;
        private IMapper mapper;

        public UpdateVehicleCategoryHandler(IVehicleCategoryService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<VehicleCategoryDto> Execute(UpdateVehicleCategoryCommand command, User? user) {
            var cat = await service.FindById(command.Id);

            if (cat == null) {
                throw new EntityNotFoundException();
            }

            if (cat.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            await service.Update(
                cat,
                new UpdateVehicleCategory() {
                    Name = command.Name,
                    Description = command.Description
                },
                user!
            );

            return mapper.Map<VehicleCategory, VehicleCategoryDto>(cat);
        }
    }
}