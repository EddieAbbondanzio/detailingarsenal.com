using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Application.ProductCatalog {
    [TestClass]
    public class ReviewCreateHandlerTests {
        [TestMethod]
        public async Task ExecuteCallsSpec() {
            var mockRepo = new Mock<IReviewRepo>();
            var mockSpec = new Mock<NoReviewAlreadyByUser>(mockRepo.Object);
            var handler = new ReviewCreateHandler(mockRepo.Object, mockSpec.Object);

            await handler.Execute(
                new ReviewCreateCommand(Guid.NewGuid(), 5, 10, 10, "Title", "Body"),
                new User("authid", "email", "username")
            );

            mockSpec.Verify(s => s.CheckAndThrow(It.IsAny<(Guid UserId, Guid PadId)>()), Times.Once);
        }
    }
}