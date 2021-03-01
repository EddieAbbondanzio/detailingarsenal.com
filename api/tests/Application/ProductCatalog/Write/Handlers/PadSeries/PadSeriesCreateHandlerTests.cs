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
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null, null);
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
                PolisherType.DualAction,
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadCreateOrUpdate[] {
                    new PadCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false, null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0, new PartNumberCreateOrUpdate[] { new PartNumberCreateOrUpdate(null, "value", "NOTES")}.ToList())
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);

            Assert.AreEqual(1, series.Count);
            var s = series[0];

            Assert.AreEqual("Name", s.Name);
            Assert.AreEqual(brandId, s.BrandId);
            Assert.AreEqual(PolisherType.DualAction, s.PolisherTypes[0]);

            Assert.AreEqual(s.Sizes[0].Diameter, new Measurement(1.2f, "in"));

            Assert.AreEqual("Color", s.Pads[0].Name);
            Assert.AreEqual(PadCategory.Cutting, s.Pads[0].Category[0]);
            Assert.AreEqual(PadTexture.Dimpled, s.Pads[0].Texture);
            Assert.AreEqual(PadMaterial.Foam, s.Pads[0].Material);
            Assert.AreEqual(1, s.Pads[0].Options.Count);
            Assert.AreEqual("value", s.Pads[0].Options[0].PartNumbers[0].Value);
            Assert.AreEqual("NOTES", s.Pads[0].Options[0].PartNumbers[0].Notes);
        }

        [TestMethod]
        public async Task ExecuteChecksSpec() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null, null);
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
                PolisherType.DualAction | PolisherType.ForcedRotation,
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadCreateOrUpdate[] {
                    new PadCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false, null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0)
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);
            mockSpec.Verify(s => s.CheckAndThrow(It.IsAny<PadSeries>()), Times.Once);
        }

        [TestMethod]
        public async Task ExecuteSavesSeries() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null, null);
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
                PolisherType.DualAction,
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadCreateOrUpdate[] {
                    new PadCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0)
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);
            mockRepo.Verify(s => s.Add(It.IsAny<PadSeries>()), Times.Once);
        }

        [TestMethod]
        public async Task ExecuteReturnsId() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null, null);
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
                PolisherType.DualAction,
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1.2f, "in"))
                }.ToList(),
                new PadCreateOrUpdate[] {
                    new PadCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0)
                    }.ToList())
                }.ToList()
            );

            var id = await h.Execute(c, null);
            Assert.AreEqual(series[0].Id, id);
        }

        [TestMethod]
        public async Task ExecuteOrdersSizesDescendingByDiameter() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null, null);
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
                PolisherType.DualAction,
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(2f, "in")),
                    new PadSizeCreateOrUpdate(null, new Measurement(1f, "in")),
                    new PadSizeCreateOrUpdate(null, new Measurement(3f, "in"))
                }.ToList(),
                new PadCreateOrUpdate[] {
                    new PadCreateOrUpdate(null, "Color", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0)
                    }.ToList())
                }.ToList()
            );

            await h.Execute(c, null);
            Assert.AreEqual(3f, series[0].Sizes[0].Diameter.Amount);
            Assert.AreEqual(2f, series[0].Sizes[1].Diameter.Amount);
            Assert.AreEqual(1f, series[0].Sizes[2].Diameter.Amount);
        }

        [TestMethod]
        public async Task ExecuteOrdersColorByName() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null, null);
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
                PolisherType.DualAction,
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1f, "in")),
                }.ToList(),
                new PadCreateOrUpdate[] {
                    new PadCreateOrUpdate(null, "B", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0)
                    }.ToList()),
                    new PadCreateOrUpdate(null, "A", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0)
                    }.ToList()),
                    new PadCreateOrUpdate(null, "C", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0)
                    }.ToList())
                }.ToList()
            );

            await h.Execute(c, null);
            Assert.AreEqual("A", series[0].Pads[0].Name);
            Assert.AreEqual("B", series[0].Pads[1].Name);
            Assert.AreEqual("C", series[0].Pads[2].Name);
        }

        [TestMethod]
        public async Task ExecuteOrdersOptionsByDiameterDescending() {
            var mockSpec = new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null, null);
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
                PolisherType.DualAction,
                new PadSizeCreateOrUpdate[] {
                    new PadSizeCreateOrUpdate(null, new Measurement(1f, "in")),
                    new PadSizeCreateOrUpdate(null, new Measurement(2f, "in")),
                    new PadSizeCreateOrUpdate(null, new Measurement(3f, "in")),
                }.ToList(),
                new PadCreateOrUpdate[] {
                    new PadCreateOrUpdate(null, "B", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOptionCreateOrUpdate[] {
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),0),
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),2),
                        new PadOptionCreateOrUpdate(Guid.NewGuid(),1),
                    }.ToList())
                }.ToList()
            );

            await h.Execute(c, null);

            Assert.AreEqual(3f, series[0].Sizes.Find(s => s.Id == series[0].Pads[0].Options[0].PadSizeId)!.Diameter.Amount);
            Assert.AreEqual(2f, series[0].Sizes.Find(s => s.Id == series[0].Pads[0].Options[1].PadSizeId)!.Diameter.Amount);
            Assert.AreEqual(1f, series[0].Sizes.Find(s => s.Id == series[0].Pads[0].Options[2].PadSizeId)!.Diameter.Amount);
        }
    }
}