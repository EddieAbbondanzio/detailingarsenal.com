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
    public class PadSeriesHasSizesSpecificationTests {
        [TestMethod]
        public async Task RejectsNoSizes() {
            var s = new PadSeries("Name", Guid.NewGuid(), PadMaterial.Foam, PadTexture.Dimpled);

            var res = await new PadSeriesHasSizesSpecification().Check(s);
            Assert.IsFalse(res.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsSizes() {
            var s = new PadSeries("Name", Guid.NewGuid(), PadMaterial.Foam, PadTexture.Dimpled);
            s.Sizes.Add(new PadSize(new Measurement(1, MeasurementUnit.Inches)));

            var res = await new PadSeriesHasSizesSpecification().Check(s);
            Assert.IsTrue(res.IsSatisfied);
        }
    }
}