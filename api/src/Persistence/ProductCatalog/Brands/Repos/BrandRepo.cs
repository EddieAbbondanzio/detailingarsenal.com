using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class BrandRepo : DatabaseInteractor, IBrandRepo {
        public BrandRepo(IDatabase database) : base(database) { }

        public async Task<Brand?> FindById(Guid id) {
            var brandModel = await Connection.QueryFirstOrDefaultAsync(
                @"select * from brands where id = @Id;",
                new { Id = id }
            );

            return new Brand(brandModel.Id, brandModel.Name);
        }

        public async Task<Brand> FindByName(string name) {
            var brandModel = await Connection.QueryFirstOrDefaultAsync(
                @"select * from brands where name = @Name;",
                new { Name = name }
            );

            return new Brand(brandModel.Id, brandModel.Name);
        }

        public async Task Add(Brand entity) {
            await Connection.ExecuteAsync(@"insert into brands (id, name) values (@Id, @Name);", entity);
        }

        public async Task Update(Brand entity) {
            await Connection.ExecuteAsync(
                @"update brands set name = @Name where id = @Id;", entity
            );
        }

        public async Task Delete(Brand entity) {
            await Connection.ExecuteAsync(@"delete from brands where id = @Id;", entity);
        }
    }
}