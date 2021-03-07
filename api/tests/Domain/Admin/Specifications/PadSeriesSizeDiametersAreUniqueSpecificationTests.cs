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
    public class PadSeriesSizeDiametersAreUniqueSpecificationTests {
        [TestMethod]
        public async Task RejectsDuplicateSizes() {
            var sizes = new List<PadSize>();
            sizes.Add(new PadSize(new Measurement(1, MeasurementUnit.Inches)));
            sizes.Add(new PadSize(new Measurement(1, MeasurementUnit.Inches)));

            var series = new PadSeries("test", Guid.NewGuid(), new(),
            sizes, new()
            );

            var res = await new PadSeriesPadSizeDiametersAreUniqueSpecification().Check(series);
            Assert.AreEqual(false, res.IsSatisfied);
        }

        [TestMethod]
        public async Task AllowsDuplicateDiameterWithDifferentUnits() {
            var sizes = new List<PadSize>();
            sizes.Add(new PadSize(new Measurement(1, MeasurementUnit.Inches)));
            sizes.Add(new PadSize(new Measurement(1, MeasurementUnit.Millimeters)));

            var series = new PadSeries("test", Guid.NewGuid(), new(),
            sizes, new()
            );

            var res = await new PadSeriesPadSizeDiametersAreUniqueSpecification().Check(series);
            Assert.AreEqual(true, res.IsSatisfied);
        }

        [TestMethod]
        public async Task AllowsDifferentSizes() {
            var sizes = new List<PadSize>();
            sizes.Add(new PadSize(new Measurement(1, MeasurementUnit.Inches)));
            sizes.Add(new PadSize(new Measurement(2, MeasurementUnit.Inches)));

            var series = new PadSeries("test", Guid.NewGuid(), new(),
            sizes, new()
            );

            var res = await new PadSeriesPadSizeDiametersAreUniqueSpecification().Check(series);
            Assert.AreEqual(true, res.IsSatisfied);
        }
    }
}
