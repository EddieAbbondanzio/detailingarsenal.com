using System;
using System.Collections.Generic;
using System.Linq;
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

            Assert.AreEqual(1.2, m!.Amount, 0.001);
            Assert.AreEqual(MeasurementUnit.Inches, m.Unit);
        }

        [TestMethod]
        public void ToInchesIgnoresInches() {
            var m = new Measurement(1, "in");
            Assert.AreEqual(1f, m.ToInches().Amount, 0.001f);
        }

        [TestMethod]
        public void ToInchesConvertsMillimeters() {
            var m = new Measurement(25.4f, "mm");
            Assert.AreEqual(1f, m.ToInches().Amount, 0.001f);
        }

        [TestMethod]
        public void ToMillimetersIgnoresMillimeters() {
            var m = new Measurement(25.4f, "mm");
            Assert.AreEqual(25.4f, m.ToMillimeters().Amount, 0.001f);
        }

        [TestMethod]
        public void ToMillimetersConvertsInches() {
            var m = new Measurement(2, "in");
            Assert.AreEqual(50.8f, m.ToMillimeters().Amount, 0.001f);
        }

        [TestMethod]
        public void CompareToOrdersFromMinToMax() {
            var amounts = new Measurement[] {
                new Measurement(5.0f, "in"),
                new Measurement(3, "in"),
                new Measurement(4, "in")
            }.ToList();

            amounts.Sort();

            Assert.AreEqual(3, amounts[0].Amount, 0.001f);
            Assert.AreEqual(4, amounts[1].Amount, 0.001f);
            Assert.AreEqual(5, amounts[2].Amount, 0.001f);
        }

        [TestMethod]
        public void CompareToHandlesDifferentUnits() {
            var amounts = new Measurement[] {
                new Measurement(5.0f, "in"),
                new Measurement(23, "mm"),
                new Measurement(14, "mm")
            }.ToList();

            amounts.Sort();

            Assert.AreEqual(14, amounts[0].Amount, 0.001f);
            Assert.AreEqual(23, amounts[1].Amount, 0.001f);
            Assert.AreEqual(5, amounts[2].Amount, 0.001f);
        }
    }
}