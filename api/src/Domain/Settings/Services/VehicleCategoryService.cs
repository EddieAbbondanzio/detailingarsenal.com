using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    public interface IVehicleCategoryService : IService {
        Task<VehicleCategory?> FindById(Guid id);
        Task<VehicleCategory> Create(CreateVehicleCategory create, User user);
        Task Update(VehicleCategory category, UpdateVehicleCategory update, User user);
        Task Delete(VehicleCategory category, User user);
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

        public async Task<VehicleCategory?> FindById(Guid id) {
            return await repo.FindById(id);
        }

        public async Task<VehicleCategory> Create(CreateVehicleCategory create, User user) {
            var cat = VehicleCategory.Create(create, user);

            await uniqueNameSpec.CheckAndThrow(cat);
            await repo.Add(cat);

            return cat;
        }

        public async Task Update(VehicleCategory category, UpdateVehicleCategory update, User user) {
            category.Name = update.Name;
            category.Description = update.Description;

            await uniqueNameSpec.CheckAndThrow(category);
            await repo.Update(category);
        }

        public async Task Delete(VehicleCategory category, User user) {
            await notInUseSpec.CheckAndThrow(category);
            await repo.Delete(category);
        }
    }
}