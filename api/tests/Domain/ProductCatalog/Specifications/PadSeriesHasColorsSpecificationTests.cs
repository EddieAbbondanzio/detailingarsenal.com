using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Domain.ProductCatalog {
    [TestClass]
    public class PadSeriesHasColorsSpecificationTests {
        [TestMethod]
        public async Task RejectsNoColors() {
            var s = new PadSeries("Name", Guid.NewGuid(), PadMaterial.Foam, PadTexture.Dimpled, new(), new(), new());
            var satisified = await new PadSeriesHasColorsSpecification().Check(s);

            Assert.IsFalse(satisified.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsColors() {
            var s = new PadSeries("Name", Guid.NewGuid(), PadMaterial.Foam, PadTexture.Dimpled, new(), new(), new());
            s.Colors.Add(new PadColor("Color", PadCategory.Cutting));

            var satisified = await new PadSeriesHasColorsSpecification().Check(s);
            Assert.IsTrue(satisified.IsSatisfied);
        }
    }
}