using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain.Clients {
    [TestClass]
    public class ClientTests {
        [TestMethod]
        public void CreateSetsUserId() {
            User u = new User("auth0", "email@fake.com", "username");

            var c = Client.Create(u.Id, "Bert", null, null);
            Assert.AreNotEqual(c.UserId, Guid.Empty);
        }
    }
}
