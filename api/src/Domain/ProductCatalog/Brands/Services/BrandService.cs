using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.ProductCatalog {
    public interface IBrandService : IService {
        Task<Brand> GetById(Guid id);
        Task<Brand> Create(BrandCreate create, User user);
        Task<Brand> Update(Brand brand, BrandUpdate update, User user);
        Task Delete(Brand brand, User user);
    }

    public class BrandService : IBrandService {
        IBrandRepo repo;
        BrandNameUniqueSpecification unique;

        public BrandService(IBrandRepo repo, BrandNameUniqueSpecification unique) {
            this.repo = repo;
            this.unique = unique;
        }

        public async Task<Brand> GetById(Guid id) {
            var brand = await repo.FindById(id);
            return brand ?? throw new EntityNotFoundException();
        }

        public async Task<Brand> Create(BrandCreate create, User user) {
            var brand = new Brand(create.Name);

            await unique.CheckAndThrow(brand);
            await repo.Add(brand);

            return brand;
        }

        public async Task<Brand> Update(Brand brand, BrandUpdate update, User user) {
            brand.Name = update.Name;

            await unique.CheckAndThrow(brand);
            await repo.Update(brand);
            return brand;
        }

        public async Task Delete(Brand brand, User user) {
            await repo.Delete(brand);
        }
    }
}