using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain.Admin.ProductCatalog {
    [TestClass]
    public class MeasurementUnitTests {
        [TestMethod]
        public void InchesHasJsonValueAttribute() {
            var a = MeasurementUnit.Inches.GetCustomAttribute<JsonValueAttribute>();
            Assert.IsInstanceOfType(a, typeof(JsonValueAttribute));
            Assert.AreEqual(a!.Value, "in");
        }

        [TestMethod]
        public void MillimetersHasJsonValueAttribute() {
            var a = MeasurementUnit.Millimeters.GetCustomAttribute<JsonValueAttribute>();

            Assert.IsInstanceOfType(a, typeof(JsonValueAttribute));
            Assert.AreEqual(a!.Value, "mm");
        }
    }
}