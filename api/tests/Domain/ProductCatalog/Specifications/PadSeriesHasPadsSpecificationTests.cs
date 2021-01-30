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
    public class PadSeriesHasPadsSpecificationTests {
        [TestMethod]
        public async Task RejectsNoPads() {
            var s = new PadSeries("Name", Guid.NewGuid(), new(), new(), new());
            var satisified = await new PadSeriesHasPadsSpecification().Check(s);

            Assert.IsFalse(satisified.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsPads() {
            var s = new PadSeries("Name", Guid.NewGuid(), new(), new(), new());
            s.Pads.Add(new Pad("Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false));

            var satisified = await new PadSeriesHasPadsSpecification().Check(s);
            Assert.IsTrue(satisified.IsSatisfied);
        }
    }
}