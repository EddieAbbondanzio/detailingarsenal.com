using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class BrandRepo : DatabaseInteractor, IBrandRepo {
        public BrandRepo(IDatabase database) : base(database) { }

        public async Task<Brand?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                var brandModel = await conn.QueryFirstOrDefaultAsync<BrandModel>(
                    @"select * from brands where id = @Id;",
                    new { Id = id }
                );

                if (brandModel == null) {
                    return null;
                }

                return new Brand(brandModel.Id, brandModel.Name);
            }
        }

        public async Task<Brand?> FindByName(string name) {
            using (var conn = OpenConnection()) {
                var brandModel = await conn.QueryFirstOrDefaultAsync<BrandModel>(
                    @"select * from brands where name = @Name;",
                    new { Name = name }
                );

                if (brandModel == null) {
                    return null;
                }

                return new Brand(brandModel.Id, brandModel.Name);
            }
        }

        public async Task Add(Brand entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(@"insert into brands (id, name) values (@Id, @Name);", entity);
            }
        }
        public async Task Update(Brand entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"update brands set name = @Name where id = @Id;", entity
                );
            }
        }

        public async Task Delete(Brand entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(@"delete from brands where id = @Id;", entity);
            }
        }
    }
}