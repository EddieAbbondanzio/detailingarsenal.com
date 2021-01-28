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
    public class PadSeriesHasOptionsForEveryPadSpecificationTests {
        [TestMethod]
        public async Task RejectsPadsWithNoOptions() {
            var s = new PadSeries("Name", Guid.NewGuid());
            s.Pads.Add(new Pad("Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));

            var satisified = await new PadSeriesHasOptionsForEveryPadSpecification().Check(s);
            Assert.IsFalse(satisified.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsPadsWithOption() {
            var s = new PadSeries("Name", Guid.NewGuid());
            s.Pads.Add(new Pad("Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));
            s.Pads[0].Options.Add(new PadOption(Guid.NewGuid()));

            var res = await new PadSeriesHasOptionsForEveryPadSpecification().Check(s);
            Assert.IsTrue(res.IsSatisfied);
        }

        [TestMethod]
        public async Task RejectsPadsWithNoOptionsWhenSeveralColors() {
            var s = new PadSeries("Name", Guid.NewGuid());
            s.Pads.Add(new Pad("Color2", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));
            s.Pads.Add(new Pad("Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red));
            s.Pads[0].Options.Add(new PadOption(Guid.NewGuid()));

            var res = await new PadSeriesHasOptionsForEveryPadSpecification().Check(s);
            Assert.IsFalse(res.IsSatisfied);
        }
    }
}