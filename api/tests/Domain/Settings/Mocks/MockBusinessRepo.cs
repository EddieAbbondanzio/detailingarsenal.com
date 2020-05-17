using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Tests.Domain {
    public class MockBusinessRepo : IBusinessRepo {
        private List<Business> businesses = new List<Business>();

#pragma warning disable 1998
        public async Task Add(Business entity) {
            businesses.Add(entity);
        }

        public async Task Delete(Business entity) {
            businesses.RemoveAll(b => b.Id == entity.Id);
        }

        public async Task<Business?> FindById(Guid id) {
            return businesses.FirstOrDefault(b => b.Id == id);
        }

        public async Task<Business> FindByUser(User user) {
            return businesses.FirstOrDefault(b => b.UserId == user.Id);
        }

        public async Task Update(Business entity) {
            // REEEEE
        }
#pragma warning restore 1998
    }
}