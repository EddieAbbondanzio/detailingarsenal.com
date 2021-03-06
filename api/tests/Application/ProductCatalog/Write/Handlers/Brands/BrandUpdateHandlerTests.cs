using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application.Admin.ProductCatalog;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Application.ProductCatalog {
    [TestClass]
    public class BrandUpdateHandlerTests {
        [TestMethod]
        public async Task UpdateUpdatesName() {
            var brand = new Brand("Name");

            var mockRepo = new Mock<IBrandRepo>();
            mockRepo.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<Brand?>(brand));
            var mockSpec = new Mock<BrandNameUniqueSpecification>(null);

            var h = new BrandUpdateHandler(
                mockRepo.Object,
                mockSpec.Object
            );

            await h.Execute(new BrandUpdateCommand(brand.Id, "New Name"));

            Assert.AreEqual("New Name", brand.Name);
        }
    }
}