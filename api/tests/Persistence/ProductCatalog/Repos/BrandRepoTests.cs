using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Persistence.Admin.ProductCatalog;
using DetailingArsenal.Persistence.ProductCatalog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Persistence.ProductCatalog {

    [TestClass, TestCategory("Integration")]
    public class BrandRepoTests : DatabaseIntegrationTests {
        [TestMethod]
        // ignore
        public async Task FindByIdFinds() {
            using (var conn = OpenConnection()) {
                var b = new Brand("Name");
                await conn.ExecuteAsync("insert into brands (id, name) values (@Id, @Name);", b);

                var repo = new BrandRepo(Database);
                var found = (await repo.FindById(b.Id))!;

                Assert.AreEqual(b.Id, found.Id);
                Assert.AreEqual(b.Name, found.Name);
            }
        }

        [TestMethod]
        public async Task FindByIdReturnsNullIfNothing() {
            using (var conn = OpenConnection()) {
                var b = new Brand("Name");
                await conn.ExecuteAsync("insert into brands (id, name) values (@Id, @Name);", b);

                var repo = new BrandRepo(Database);
                var found = (await repo.FindById(Guid.NewGuid()));

                Assert.IsNull(found);
            }
        }


        [TestMethod]
        public async Task FindByNameFinds() {
            using (var conn = OpenConnection()) {
                var b = new Brand("Name23");
                await conn.ExecuteAsync("insert into brands (id, name) values (@Id, @Name);", b);

                var repo = new BrandRepo(Database);
                var found = (await repo.FindByName(b.Name))!;

                Assert.AreEqual(b.Id, found.Id);
                Assert.AreEqual(b.Name, found.Name);
            }
        }

        [TestMethod]
        public async Task FindByNameReturnsNullIfNothing() {
            using (var conn = OpenConnection()) {
                var b = new Brand("Name");
                await conn.ExecuteAsync("insert into brands (id, name) values (@Id, @Name);", b);

                var repo = new BrandRepo(Database);
                var found = await repo.FindByName("cat");

                Assert.IsNull(found);
            }
        }


        [TestMethod]
        public async Task AddSavesBrandToDatabase() {
            var brandRepo = new BrandRepo(DatabaseManager.Database);
            var b = new Brand("Name");

            await brandRepo.Add(b);

            using (var conn = OpenConnection()) {
                var raw = await conn.QueryFirstOrDefaultAsync(
                    @"select * from brands where id = @Id;", b
                );

                Assert.AreEqual(b.Id, raw.id);
                Assert.AreEqual(b.Name, raw.name);
            }
        }

        [TestMethod]
        public async Task UpdateUpdatesName() {
            using (var conn = OpenConnection()) {
                var b = new Brand("Name");
                await conn.ExecuteAsync("insert into brands (id, name) values (@Id, @Name);", b);

                var repo = new BrandRepo(Database);
                b.Name = "Name2";
                await repo.Update(b);

                var newName = await conn.QueryFirstAsync<string>("Select name from brands where id = @Id;", b);
                Assert.AreEqual("Name2", b.Name);

            }
        }

        [TestMethod]
        public async Task DeleteRemovesBrand() {
            using (var conn = OpenConnection()) {
                var b = new Brand("Name");
                await conn.ExecuteAsync("insert into brands (id, name) values (@Id, @Name);", b);

                var repo = new BrandRepo(Database);
                await repo.Delete(b);

                var count = await conn.QueryFirstAsync<int>("Select count(*) from brands where id = @Id;", b);
                Assert.AreEqual(0, count);

            }
        }

        [ClassCleanup]
        public static async Task ClearDatabase() {
            using (var conn = DatabaseManager.Database.OpenConnection()) {
                await conn.ExecuteAsync(
                    "delete from pad_series; delete from brands;"
                );
            }
        }
    }
}
