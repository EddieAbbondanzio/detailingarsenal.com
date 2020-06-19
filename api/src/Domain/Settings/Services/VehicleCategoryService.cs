using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    public interface IVehicleCategoryService : IService {
        Task<VehicleCategory> Create(User user, CreateVehicleCategory create);
        Task Update(User user, VehicleCategory category, UpdateVehicleCategory update);
        Task Delete(User user, VehicleCategory category);
    }
}