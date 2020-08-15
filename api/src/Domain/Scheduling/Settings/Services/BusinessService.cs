using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IBusinessService : IService {
        Task<Business> GetById(Guid id);
        Task<Business> GetByUser(User user);
        Task<Business> GetOrCreateForUser(User user);
        Task<Business> CreateDefault(User user);
        Task Update(Business business, BusinessUpdate update);
        Task Delete(Business business);
    }

    public class BusinessService : IBusinessService {
        public IBusinessRepo repo;

        public BusinessService(IBusinessRepo repo) {
            this.repo = repo;
        }

        public async Task<Business> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<Business> GetByUser(User user) {
            return await repo.FindByUser(user) ?? throw new EntityNotFoundException();
        }

        public async Task<Business> GetOrCreateForUser(User user) {
            var b = await repo.FindByUser(user);

            if (b == null) {
                b = Business.Create(user.Id);
                await repo.Add(b);
            }

            return b;
        }

        public async Task<Business> CreateDefault(User user) {
            var b = Business.Create(user.Id);
            await repo.Add(b);

            return b;
        }

        public async Task Update(Business business, BusinessUpdate update) {
            business.Name = update.Name;
            business.Address = update.Address;
            business.Phone = update.Phone;

            await repo.Update(business);
        }

        public async Task Delete(Business business) {
            await repo.Delete(business);
        }
    }
}