using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "update", Scope = "services")]
    [Validation(typeof(UpdateServiceValidator))]
    public class UpdateServiceHandler : ActionHandler<UpdateServiceCommand, ServiceView> {
        IServiceService service;
        private IMapper mapper;

        public UpdateServiceHandler(IServiceService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ServiceView> Execute(UpdateServiceCommand input, User? user) {
            var service = await this.service.GetById(input.Id);

            if (!service.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await this.service.Update(
                service,
                new ServiceUpdate(
                    input.Name,
                    input.Description,
                    input.PricingMethod,
                    input.Configurations.Select(c => new ServiceConfigurationUpdate(
                        c.VehicleCategoryId,
                        c.Price,
                        c.Duration
                    )).ToList()
                )
            );

            return mapper.Map<Service, ServiceView>(service);
        }
    }
}