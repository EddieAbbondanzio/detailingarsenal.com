using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Persistence.ProductCatalog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Persistence.ProductCatalog {
    [TestClass, TestCategory("Integration")]
    public class PadSeriesFilterTests : DatabaseIntegrationTests {
        [ClassInitialize]
        public static async Task InsertTestData(TestContext context) {
            // Ensure actually empty
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync("delete from pad_series;");
                await conn.ExecuteAsync("delete from brands;");
            }

            // Insert brands
            var brandRepo = new BrandRepo(Database);
            var b1 = new Brand("Brand1");
            var b2 = new Brand("Brand2");
            var b3 = new Brand("Brand3");

            await brandRepo.Add(b1);
            await brandRepo.Add(b2);
            await brandRepo.Add(b3);

            // Insert pad series
            var ps1 = new PadSeries("Beta", b1.Id, new(new[] { PolisherType.DualAction }));
            ps1.Sizes.Add(new PadSize(new Measurement(1, "in")));
            ps1.Pads.Add(new Pad("Pad", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false));
            ps1.Pads[0].Options.Add(new PadOption(ps1.Sizes[0].Id));

            var ps2 = new PadSeries("Zoo", b1.Id, new(new[] { PolisherType.DualAction }));
            ps2.Sizes.Add(new PadSize(new Measurement(1, "in")));
            ps2.Pads.Add(new Pad("Pad2", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false));
            ps2.Pads[0].Options.Add(new PadOption(ps2.Sizes[0].Id));

            var padSeriesRepo = new PadSeriesRepo(Database);
            await padSeriesRepo.Add(ps1);
            await padSeriesRepo.Add(ps2);
        }

        [TestMethod]
        public async Task Read() {
            var reader = new PadSeriesFilterReader(Database);
            var filter = await reader.Read();

            Assert.AreEqual(1, filter.Brands.Count()); // No empty brands
            Assert.AreEqual(2, filter.Series.Count());
            Assert.AreEqual("Brand1", filter.Brands.ElementAt(0).Name);
            Assert.AreEqual("Beta", filter.Series.ElementAt(0).Name);
            Assert.AreEqual("Zoo", filter.Series.ElementAt(1).Name);
        }


        [ClassCleanup]
        public static async Task ClearDatabase() {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync("delete from pad_series;");
                await conn.ExecuteAsync("delete from brands;");
            }
        }
    }
}
