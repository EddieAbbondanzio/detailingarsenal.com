using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    public interface IVehicleCategoryService : IService {
        Task<VehicleCategory> GetById(Guid id);
        Task<List<VehicleCategory>> GetByUser(User user);
        Task<VehicleCategory> Create(CreateVehicleCategory create, User user);
        Task Update(VehicleCategory category, UpdateVehicleCategory update);
        Task Delete(VehicleCategory category);
    }

    public class VehicleCategoryService : IVehicleCategoryService {
        private VehicleCategoryNameUniqueSpecification uniqueNameSpec;
        private VehicleCategoryNotInUseSpecification notInUseSpec;
        private IVehicleCategoryRepo repo;

        public VehicleCategoryService(VehicleCategoryNameUniqueSpecification uniqueNameSpec, VehicleCategoryNotInUseSpecification notInUseSpec, IVehicleCategoryRepo repo) {
            this.uniqueNameSpec = uniqueNameSpec;
            this.notInUseSpec = notInUseSpec;
            this.repo = repo;
        }

        public async Task<VehicleCategory> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<List<VehicleCategory>> GetByUser(User user) {
            return await repo.FindByUser(user);
        }

        public async Task<VehicleCategory> Create(CreateVehicleCategory create, User user) {
            var cat = VehicleCategory.Create(create, user);

            await uniqueNameSpec.CheckAndThrow(cat);
            await repo.Add(cat);

            return cat;
        }

        public async Task Update(VehicleCategory category, UpdateVehicleCategory update) {
            category.Name = update.Name;
            category.Description = update.Description;

            await uniqueNameSpec.CheckAndThrow(category);
            await repo.Update(category);
        }

        public async Task Delete(VehicleCategory category) {
            await notInUseSpec.CheckAndThrow(category);
            await repo.Delete(category);
        }
    }
}