using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain.Settings {
    [TestClass]
    public class HoursOfOperationTests {
        [TestMethod]
        public void DefaultDaysAreSet() {
            var hours = HoursOfOperation.Create(Guid.NewGuid());
            Assert.AreNotEqual(hours.Days.Count, 0);
        }
    }
}
