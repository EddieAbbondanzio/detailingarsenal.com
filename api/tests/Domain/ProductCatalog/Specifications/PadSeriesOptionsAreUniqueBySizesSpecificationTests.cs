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
    public class PadSeriesOptionsAreUniqueBySizesSpecificationTests {
        [TestMethod]
        public async Task RejectsOptionsOnSameColorWithTheSameSize() {
            var s = new PadSeries("Name", Guid.NewGuid());
            s.Pads.Add(new Pad("Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));

            var sizeId = Guid.NewGuid();

            s.Pads[0].Options.Add(new PadOption(sizeId));
            s.Pads[0].Options.Add(new PadOption(sizeId));

            var res = await new PadSeriesOptionsAreUniqueBySizesSpecification().Check(s);
            Assert.IsFalse(res.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsOptionsWithDifferentSizes() {
            var s = new PadSeries("Name", Guid.NewGuid());
            s.Pads.Add(new Pad("Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));

            s.Pads[0].Options.Add(new PadOption(Guid.NewGuid()));
            s.Pads[0].Options.Add(new PadOption(Guid.NewGuid()));

            var res = await new PadSeriesOptionsAreUniqueBySizesSpecification().Check(s);
            Assert.IsTrue(res.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsOptionsWithSameSizeOnDifferentColors() {
            var s = new PadSeries("Name", Guid.NewGuid());
            s.Pads.Add(new Pad("Color1", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));
            s.Pads.Add(new Pad("Color2", PadCategory.Finishing, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));

            var sizeId = Guid.NewGuid();

            s.Pads[0].Options.Add(new PadOption(sizeId));
            s.Pads[1].Options.Add(new PadOption(sizeId));

            var res = await new PadSeriesOptionsAreUniqueBySizesSpecification().Check(s);
            Assert.IsTrue(res.IsSatisfied);
        }
    }
}