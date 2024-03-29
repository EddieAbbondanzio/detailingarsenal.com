using System;
using System.Collections.Generic;
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
    public class PadSeriesHasSizesSpecificationTests {
        [TestMethod]
        public async Task RejectsNoSizes() {
            var s = new PadSeries("Name", Guid.NewGuid());

            var res = await new PadSeriesHasSizesSpecification().Check(s);
            Assert.IsFalse(res.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsSizes() {
            var s = new PadSeries("Name", Guid.NewGuid());
            s.Sizes.Add(new PadSize(new Measurement(1, MeasurementUnit.Inches)));

            var res = await new PadSeriesHasSizesSpecification().Check(s);
            Assert.IsTrue(res.IsSatisfied);
        }
    }
}