using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "create", Scope = "services")]
    public class CreateServiceHandler : ActionHandler<CreateServiceCommand, ServiceDto> {
        private ServiceNameUniqueSpecification specification;
        private IServiceRepo repo;
        private IMapper mapper;

        public CreateServiceHandler(ServiceNameUniqueSpecification specification, IServiceRepo repo, IMapper mapper) {
            this.specification = specification;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<ServiceDto> Execute(CreateServiceCommand input, User? user) {
            var service = Service.Create(
                user!.Id,
                input.Name,
                input.Description,
                input.PricingMethod
            );

            service.Configurations = input.Configurations.Select(c => ServiceConfiguration.Create(
                service.Id,
                c.VehicleCategoryId,
                c.Price,
                c.Duration
            )).ToList();

            await specification.CheckAndThrow(service);

            await repo.Add(service);
            return mapper.Map<Service, ServiceDto>(service);
        }
    }
}