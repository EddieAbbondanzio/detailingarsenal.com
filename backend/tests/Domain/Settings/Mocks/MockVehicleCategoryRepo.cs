using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Tests.Domain {
    public class MockVehicleCategoryRepo : IVehicleCategoryRepo {
        public List<VehicleCategory> Categories { get; set; } = new List<VehicleCategory>();

#pragma warning disable 1998
        public async Task<VehicleCategory?> FindById(Guid id) {
            return Categories.Find(c => c.Id == id);
        }

        public async Task<VehicleCategory?> FindByName(string name) {
            return Categories.Find(c => c.Name == name);
        }

        public async Task<List<VehicleCategory>> FindByUser(User user) {
            return Categories.FindAll(c => c.UserId == user.Id).ToList();
        }

        public async Task Add(VehicleCategory entity) {
            Categories.Add(entity);
        }

        public async Task Update(VehicleCategory entity) {
            // REEEEE
        }

        public async Task Delete(VehicleCategory entity) {
            Categories.Remove(entity);
        }

        public Task<bool> IsInUse(VehicleCategory vehicleCategory) {
            throw new NotImplementedException();
        }
#pragma warning restore 1998
    }
}