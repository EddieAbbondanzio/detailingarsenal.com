using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain.Settings {
    [TestClass]
    public class VehicleCategoryServiceTests {
        [TestMethod]
        public async Task CreateSetsNameAndDescription() {
            var repo = Mock.Of<IVehicleCategoryRepo>();
            var service = new VehicleCategoryService(
                new VehicleCategoryNameUniqueSpecification(repo),
                new VehicleCategoryNotInUseSpecification(repo),
                repo
            );

            var user = new User("auth0", "email@fake.com", "username");


            var cat = await service.Create(
                new VehicleCategoryCreate(
                    "name",
                    "desc"
                ),
                user
            );

            Assert.AreEqual(cat.Name, "name");
            Assert.AreEqual(cat.Description, "desc");
        }

        [TestMethod]
        public async Task CreatePreventsDuplicateNames() {
            var mock = new Mock<IVehicleCategoryRepo>();
            mock.Setup(m => m.FindByName("cat")).ReturnsAsync(new VehicleCategory() { Name = "cat" });

            var repo = mock.Object;
            var service = new VehicleCategoryService(
                new VehicleCategoryNameUniqueSpecification(repo),
                new VehicleCategoryNotInUseSpecification(repo),
                repo
            );

            var user = new User("auth0", "email@fake.com", "username");


            await Assert.ThrowsExceptionAsync<SpecificationException>(
                async () => await service.Create(
                    new VehicleCategoryCreate(
                        "cat"
                    ),
                    user
                )
            );
        }

        [TestMethod]
        public async Task UpdateSetsNameAndDescription() {
            var mock = new Mock<IVehicleCategoryRepo>();

            var repo = mock.Object;
            var service = new VehicleCategoryService(
                new VehicleCategoryNameUniqueSpecification(repo),
                new VehicleCategoryNotInUseSpecification(repo),
                repo
            );

            var user = new User("auth0", "email@fake.com", "username");


            var cat = new VehicleCategory() {
                Name = "old",
                Description = "oldDesc"
            };

            await service.Update(
                cat,
                new VehicleCategoryUpdate(
                    "new",
                    "newDesc"
                )
            );

            Assert.AreEqual(cat.Name, "new");
            Assert.AreEqual(cat.Description, "newDesc");
        }

        [TestMethod]
        public async Task UpdatePreventsDuplicateNames() {
            var mock = new Mock<IVehicleCategoryRepo>();
            mock.Setup(m => m.FindByName("cat")).ReturnsAsync(new VehicleCategory() { Id = Guid.NewGuid(), Name = "cat" });

            var repo = mock.Object;
            var service = new VehicleCategoryService(
                new VehicleCategoryNameUniqueSpecification(repo),
                new VehicleCategoryNotInUseSpecification(repo),
                repo
            );

            var user = new User("auth0", "email@fake.com", "username");


            var updatingCat = new VehicleCategory() {
                Id = Guid.NewGuid(),
                Name = "old"
            };

            await Assert.ThrowsExceptionAsync<SpecificationException>(
                async () => await service.Update(
                    updatingCat,
                    new VehicleCategoryUpdate(
                        "cat"
                    )
                )
            );
        }

        [TestMethod]
        public async Task DeleteThrowsErrorIfInUse() {
            var mock = new Mock<IVehicleCategoryRepo>();
            mock.Setup(m => m.IsInUse(It.IsAny<VehicleCategory>())).ReturnsAsync(true);

            var repo = mock.Object;
            var service = new VehicleCategoryService(
                new VehicleCategoryNameUniqueSpecification(repo),
                new VehicleCategoryNotInUseSpecification(repo),
                repo
            );

            var cat = new VehicleCategory() {
                Name = "cat"
            };

            var user = new User("auth0", "email@fake.com", "username");


            await Assert.ThrowsExceptionAsync<SpecificationException>(
                async () => {
                    await service.Delete(
                        cat
                    );
                }
            );
        }
    }
}
