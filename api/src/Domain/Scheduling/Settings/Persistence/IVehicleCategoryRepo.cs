using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IVehicleCategoryRepo : IRepo<VehicleCategory> {
        Task<List<VehicleCategory>> FindByUser(User user);
        Task<VehicleCategory?> FindByName(string name);

        Task<bool> IsInUse(VehicleCategory vehicleCategory);
    }
}