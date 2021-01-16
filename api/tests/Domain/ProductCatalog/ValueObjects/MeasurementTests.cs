using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain.ProductCatalog {
    [TestClass]
    public class MeasurementTests {
        [TestMethod]
        public void ThrowsExceptionIfAmountIsNegative() {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                new Measurement(-1, MeasurementUnit.Inches);
            });
        }

        [TestMethod]
        public void JsonSerializes() {
            var json = JsonSerializer.Serialize(new Measurement(1.2f, MeasurementUnit.Inches));
            var m = JsonSerializer.Deserialize<Measurement>(json);

            Assert.AreEqual(1.2, m.Amount);
            Assert.AreEqual(MeasurementUnit.Inches, m.Unit);
        }
    }
}