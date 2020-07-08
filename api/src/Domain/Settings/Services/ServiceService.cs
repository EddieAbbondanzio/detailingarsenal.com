using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IServiceService : IService {
        Task<Service> GetById(Guid id);
        Task<List<Service>> GetByUser(User user);
        Task<Service> Create(ServiceCreate create, User user);
        Task Update(Service service, ServiceUpdate update);
        Task Delete(Service service);
    }

    public class ServiceService : IServiceService {
        IServiceRepo repo;
        ServiceNameUniqueSpecification uniqueNameSpec;
        ServiceNotInUseSpecification notInUseSpec;

        public ServiceService(IServiceRepo repo, ServiceNameUniqueSpecification uniqueNameSpec, ServiceNotInUseSpecification notInUseSpec) {
            this.repo = repo;
            this.uniqueNameSpec = uniqueNameSpec;
            this.notInUseSpec = notInUseSpec;
        }

        public async Task<Service> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<List<Service>> GetByUser(User user) {
            return await repo.FindByUser(user);
        }

        public async Task<Service> Create(ServiceCreate create, User user) {
            var service = Service.Create(
                user!.Id,
                create.Name,
                create.Description,
                create.PricingMethod
            );

            service.Configurations = create.Configurations.Select(
                c => ServiceConfiguration.Create(
                    service.Id,
                    c.VehicleCategoryId,
                    c.Price,
                    c.Duration
                )
            ).ToList();

            await uniqueNameSpec.CheckAndThrow(service);
            await repo.Add(service);

            return service;

        }
        public async Task Update(Service service, ServiceUpdate update) {
            service.Name = update.Name;
            service.Description = update.Description;
            service.PricingMethod = update.PricingMethod;

            service.Configurations = update.Configurations.Select(
                c => ServiceConfiguration.Create(
                    service.Id,
                    c.VehicleCategoryId,
                    c.Price,
                    c.Duration
                )
            ).ToList();

            await uniqueNameSpec.CheckAndThrow(service);
            await repo.Update(service);
        }

        public async Task Delete(Service service) {
            await notInUseSpec.CheckAndThrow(service);
            await repo.Delete(service);
        }
    }
}