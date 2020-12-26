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
    public class PadSeriesHasOptionsForEveryColorSpecificationTests {
        [TestMethod]
        public async Task RejectsColorWithNoOptions() {
            var s = new PadSeries("Name", Guid.NewGuid(), PadMaterial.Foam, PadTexture.Dimpled);
            s.Colors.Add(new PadColor("Color", PadCategory.Cutting));

            var satisified = await new PadSeriesHasOptionsForEveryColorSpecification().Check(s);
            Assert.IsFalse(satisified.IsSatisfied);
        }

        [TestMethod]
        public async Task AcceptsColorWithOption() {
            var s = new PadSeries("Name", Guid.NewGuid(), PadMaterial.Foam, PadTexture.Dimpled);
            s.Colors.Add(new PadColor("Color", PadCategory.Cutting));
            s.Colors[0].Options.Add(new PadOption(Guid.NewGuid()));

            var res = await new PadSeriesHasOptionsForEveryColorSpecification().Check(s);
            Assert.IsTrue(res.IsSatisfied);
        }

        [TestMethod]
        public async Task RejectsColorWithNoOptionsWhenSeveralColors() {
            var s = new PadSeries("Name", Guid.NewGuid(), PadMaterial.Foam, PadTexture.Dimpled);
            s.Colors.Add(new PadColor("Color2", PadCategory.Cutting));
            s.Colors.Add(new PadColor("Color", PadCategory.Cutting));
            s.Colors[0].Options.Add(new PadOption(Guid.NewGuid()));

            var res = await new PadSeriesHasOptionsForEveryColorSpecification().Check(s);
            Assert.IsFalse(res.IsSatisfied);
        }
    }
}