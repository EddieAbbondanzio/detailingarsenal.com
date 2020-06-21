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

            var valid = await uniqueNameSpec.Check(cat);

            if (!valid.IsSatisfied) {
                throw new VehicleCategoryNameInUseException(valid.Messages[0]);
            }

            await repo.Add(cat);

            return cat;
        }

        public async Task Update(VehicleCategory category, UpdateVehicleCategory update, User user) {
            category.Name = update.Name;
            category.Description = update.Description;

            var valid = await uniqueNameSpec.Check(category);

            if (!valid.IsSatisfied) {
                throw new VehicleCategoryNameInUseException(valid.Messages[0]);
            }

            await repo.Update(category);
        }

        public async Task Delete(VehicleCategory category, User user) {
            var valid = await notInUseSpec.Check(category);

            if (!valid.IsSatisfied) {
                throw new InvalidOperationException(valid.Messages[0]);
            }

            await repo.Delete(category);
        }
    }
}