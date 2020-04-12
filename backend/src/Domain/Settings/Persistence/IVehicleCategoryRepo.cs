using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IVehicleCategoryRepo : IRepo<VehicleCategory> {
        Task<List<VehicleCategory>> FindByUser(User user);
        Task<VehicleCategory?> FindByName(string name);

        Task<bool> IsInUse(VehicleCategory vehicleCategory);
    }
}