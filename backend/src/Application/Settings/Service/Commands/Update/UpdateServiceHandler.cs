using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class UpdateServiceHandler : ActionHandler<UpdateServiceCommand, ServiceDto> {
        private ServiceNameUniqueSpecification specification;
        private IServiceRepo repo;
        private IMapper mapper;

        public UpdateServiceHandler(ServiceNameUniqueSpecification specification, IServiceRepo repo, IMapper mapper) {
            this.specification = specification;
            this.repo = repo;
            this.mapper = mapper;
        }

        protected async override Task<ServiceDto> Execute(UpdateServiceCommand input, User? user) {
            var service = (await repo.FindById(input.Id)) ?? throw new EntityNotFoundException();

            if (service.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            service.Name = input.Name;
            service.Description = input.Description;
            service.PricingMethod = input.PricingMethod;
            service.Configurations = input.Configurations.Select(c => new ServiceConfiguration() {
                Id = Guid.NewGuid(),
                ServiceId = service.Id,
                VehicleCategoryId = c.VehicleCategoryId,
                Price = c.Price,
                Duration = c.Duration
            }).ToList();

            await specification.CheckAndThrow(service);

            await repo.Update(service);
            return mapper.Map<Service, ServiceDto>(service);
        }
    }
}