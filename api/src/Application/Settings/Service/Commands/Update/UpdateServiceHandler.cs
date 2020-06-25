using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "update", Scope = "services")]
    [Validation(typeof(UpdateServiceValidator))]
    public class UpdateServiceHandler : ActionHandler<UpdateServiceCommand, ServiceDto> {
        IServiceService service;
        private IMapper mapper;

        public UpdateServiceHandler(IServiceService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ServiceDto> Execute(UpdateServiceCommand input, User? user) {
            var service = await this.service.GetById(input.Id);

            if (!service.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await this.service.Update(
                service,
                new UpdateService(
                    input.Name,
                    input.Description,
                    input.PricingMethod,
                    input.Configurations.Select(c => new UpdateServiceConfiguration(
                        c.VehicleCategoryId,
                        c.Price,
                        c.Duration
                    )).ToList()
                )
            );

            return mapper.Map<Service, ServiceDto>(service);
        }
    }
}