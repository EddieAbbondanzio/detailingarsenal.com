using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "create", Scope = "services")]
    [Validation(typeof(CreateServiceValidator))]
    public class CreateServiceHandler : ActionHandler<CreateServiceCommand, ServiceDto> {

        private IServiceService service;
        private IMapper mapper;

        public CreateServiceHandler(IServiceService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ServiceDto> Execute(CreateServiceCommand input, User? user) {
            var s = await service.Create(
                new CreateService(
                    user!.Id,
                    input.Name,
                    input.Description,
                    input.PricingMethod,
                    input.Configurations.Select(c => new CreateServiceConfiguration(
                        c.VehicleCategoryId,
                        c.Price,
                        c.Duration
                    )).ToList()
                ),
                user!
            );

            return mapper.Map<Service, ServiceDto>(s);
        }
    }
}