using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
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
            var service = new Service() {
                Id = Guid.NewGuid(),
                UserId = user!.Id,
                Name = input.Name,
                Description = input.Description,
                PricingMethod = input.PricingMethod,
            };

            service.Configurations = input.Configurations.Select(c => new ServiceConfiguration() {
                Id = Guid.NewGuid(),
                ServiceId = service.Id,
                VehicleCategoryId = c.VehicleCategoryId,
                Price = c.Price,
                Duration = c.Duration
            }).ToList();

            await specification.CheckAndThrow(service);

            await repo.Add(service);
            return mapper.Map<Service, ServiceDto>(service);
        }
    }
}