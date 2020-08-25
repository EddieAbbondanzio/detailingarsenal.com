using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "create", Scope = "services")]
    [Validation(typeof(CreateServiceValidator))]
    public class CreateServiceHandler : ActionHandler<CreateServiceCommand, ServiceView> {

        private IServiceService service;
        private IMapper mapper;

        public CreateServiceHandler(IServiceService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ServiceView> Execute(CreateServiceCommand input, User? user) {
            var s = await service.Create(
                new ServiceCreate(
                    user!.Id,
                    input.Name,
                    input.Description,
                    input.PricingMethod,
                    input.Configurations.Select(c => new ServiceConfigurationCreate(
                        c.VehicleCategoryId,
                        c.Price,
                        c.Duration
                    )).ToList()
                ),
                user!
            );

            return mapper.Map<Service, ServiceView>(s);
        }
    }
}