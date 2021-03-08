using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Persistence.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Persistence.Shared {
    [TestClass]
    public class PadColorUtilTests {
        [TestMethod]
        public void HandlesNull() {
            Assert.IsNull(PadColorUtils.Parse(null));
        }
    }
}