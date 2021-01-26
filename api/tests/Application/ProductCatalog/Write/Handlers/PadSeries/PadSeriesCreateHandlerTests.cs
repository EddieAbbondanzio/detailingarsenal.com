using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DetailingArsenal.Tests.Application.ProductCatalog {
    [TestClass]
    public class PadSeriesCreateHandlerTests {
        [TestMethod]
        public async Task ExecuteCreatesSizes() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null);
            mockSpec.Setup(s => s.CheckAndThrow(It.IsAny<PadSeries>())).Returns(Task.FromResult(new SpecificationResult(true)));

            var mockRepo = new Mock<IPadSeriesRepo>();
            List<PadSeries> series = new();
            mockRepo.Setup(r => r.Add(Capture.In(series)));

            var h = new PadSeriesCreateHandler(
                mockSpec.Object,
                mockRepo.Object,
                Mock.Of<IImageProcessor>()
            );

            var brandId = Guid.NewGuid();

            var c = new PadSeriesCreateCommand(
                "Name",
                brandId,
                new[] { PolisherType.DualAction, PolisherType.ForcedRotation }.ToList(),
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadColorCreateOrUpdate[] {
                    new PadColorCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate() { PadSizeIndex = 0, PartNumber = "part_number"}
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);

            Assert.AreEqual(1, series.Count);
            var s = series[0];

            Assert.AreEqual("Name", s.Name);
            Assert.AreEqual(brandId, s.BrandId);
            Assert.AreEqual(2, s.PolisherTypes.Count);
            Assert.AreEqual(PolisherType.DualAction, s.PolisherTypes[0]);
            Assert.AreEqual(PolisherType.ForcedRotation, s.PolisherTypes[1]);

            Assert.AreEqual(s.Sizes[0].Diameter, new Measurement(1.2f, "in"));

            Assert.AreEqual("Color", s.Colors[0].Name);
            Assert.AreEqual(PadCategory.Cutting, s.Colors[0].Category);
            Assert.AreEqual(PadTexture.Dimpled, s.Colors[0].Texture);
            Assert.AreEqual(PadMaterial.Foam, s.Colors[0].Material);
            Assert.AreEqual(1, s.Colors[0].Options.Count);
            Assert.AreEqual("part_number", s.Colors[0].Options[0].PartNumber);
        }

        [TestMethod]
        public async Task ExecuteChecksSpec() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null);
            mockSpec.Setup(s => s.CheckAndThrow(It.IsAny<PadSeries>())).Returns(Task.FromResult(new SpecificationResult(true)));

            var mockRepo = new Mock<IPadSeriesRepo>();
            List<PadSeries> series = new();
            mockRepo.Setup(r => r.Add(Capture.In(series)));

            var h = new PadSeriesCreateHandler(
                mockSpec.Object,
                mockRepo.Object,
                Mock.Of<IImageProcessor>()
            );

            var brandId = Guid.NewGuid();

            var c = new PadSeriesCreateCommand(
                "Name",
                brandId,
                new[] { PolisherType.DualAction, PolisherType.ForcedRotation }.ToList(),
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadColorCreateOrUpdate[] {
                    new PadColorCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate() { PadSizeIndex = 0, PartNumber = "part_number"}
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);
            mockSpec.Verify(s => s.CheckAndThrow(It.IsAny<PadSeries>()), Times.Once);
        }

        [TestMethod]
        public async Task ExecuteSavesSeries() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null);
            mockSpec.Setup(s => s.CheckAndThrow(It.IsAny<PadSeries>())).Returns(Task.FromResult(new SpecificationResult(true)));

            var mockRepo = new Mock<IPadSeriesRepo>();
            List<PadSeries> series = new();
            mockRepo.Setup(r => r.Add(Capture.In(series)));

            var h = new PadSeriesCreateHandler(
                mockSpec.Object,
                mockRepo.Object,
                Mock.Of<IImageProcessor>()
            );

            var brandId = Guid.NewGuid();

            var c = new PadSeriesCreateCommand(
                "Name",
                brandId,
                new[] { PolisherType.DualAction, PolisherType.ForcedRotation }.ToList(),
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadColorCreateOrUpdate[] {
                    new PadColorCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate() { PadSizeIndex = 0, PartNumber = "part_number"}
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);
            mockRepo.Verify(s => s.Add(It.IsAny<PadSeries>()), Times.Once);
        }

        [TestMethod]
        public async Task ExecuteReturnsId() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null);
            mockSpec.Setup(s => s.CheckAndThrow(It.IsAny<PadSeries>())).Returns(Task.FromResult(new SpecificationResult(true)));

            var mockRepo = new Mock<IPadSeriesRepo>();
            List<PadSeries> series = new();
            mockRepo.Setup(r => r.Add(Capture.In(series)));

            var h = new PadSeriesCreateHandler(
                mockSpec.Object,
                mockRepo.Object,
                Mock.Of<IImageProcessor>()
            );

            var brandId = Guid.NewGuid();

            var c = new PadSeriesCreateCommand(
                "Name",
                brandId,
                new[] { PolisherType.DualAction, PolisherType.ForcedRotation }.ToList(),
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadColorCreateOrUpdate[] {
                    new PadColorCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate() { PadSizeIndex = 0, PartNumber = "part_number"}
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);
            Assert.AreEqual(series[0].Id, id);
        }
    }
}