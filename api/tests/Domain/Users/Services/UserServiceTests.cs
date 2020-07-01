using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain.Users {
    [TestClass]
    public class UserServiceTests {
        [TestMethod]
        public async Task CreateUserAddsToDatabase() {
            var eventPublisher = Mock.Of<IDomainEventPublisher>();
            var userGateway = Mock.Of<IUserGateway>();
            var repoMock = new Mock<IUserRepo>();

            var service = new UserService(
                userGateway,
                repoMock.Object,
                eventPublisher
            );

            await service.CreateAdminUser(
                "fake@mail.com",
                "password"
            );

            repoMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }

    }
}
