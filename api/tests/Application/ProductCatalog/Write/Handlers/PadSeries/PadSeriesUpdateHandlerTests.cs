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
    public class PadSeriesUpdateHandlerTests {
        [TestMethod]
        public void UpdatePadSizesExistingPadSizesAreUpdated() {
            var h = new PadSeriesUpdateHandler(
                Mock.Of<IPadSeriesRepo>(),
                new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                Mock.Of<IImageProcessor>()
            );

            var existing = new PadSize[] {
                new PadSize(new Measurement(1.0f, MeasurementUnit.Inches)),
                new PadSize(new Measurement(2.0f, MeasurementUnit.Inches))
            }.ToList();

            var updates = new PadSizeCreateOrUpdate[] {
                new PadSizeCreateOrUpdate(existing[0].Id, new Measurement(1.5f, MeasurementUnit.Inches), new Measurement(2.5f, MeasurementUnit.Inches)),
            }.ToList();

            var updated = h.UpdatePadSizes(existing, updates);

            Assert.AreEqual(new Measurement(1.5f, MeasurementUnit.Inches), updated[0].Diameter);
            Assert.AreEqual(new Measurement(2.5f, MeasurementUnit.Inches), updated[0].Thickness);
        }

        [TestMethod]
        public void UpdatePadSizesAddsNewPadSizes() {
            var h = new PadSeriesUpdateHandler(
                            Mock.Of<IPadSeriesRepo>(),
                            new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                            Mock.Of<IImageProcessor>()
                        );

            var existing = new PadSize[] {
                new PadSize(new Measurement(1.0f, MeasurementUnit.Inches)),
            }.ToList();

            var news = new PadSizeCreateOrUpdate[] {
                new PadSizeCreateOrUpdate(existing[0].Id, new Measurement(1.0f, MeasurementUnit.Inches), new Measurement(2.5f, MeasurementUnit.Inches)),
                new PadSizeCreateOrUpdate(null, new Measurement(4.0f, MeasurementUnit.Inches))
            }.ToList();

            var updated = h.UpdatePadSizes(existing, news);

            Assert.AreEqual(2, updated.Count);
            Assert.AreEqual(4.0f, updated[0].Diameter.Amount);
            Assert.IsNotNull(updated[0].Id);
        }

        [TestMethod]
        public void UpdatePadSizesExistingPadSizesAreDeletedAsNeeded() {
            var h = new PadSeriesUpdateHandler(
                Mock.Of<IPadSeriesRepo>(),
                new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                Mock.Of<IImageProcessor>()
            );

            var existing = new PadSize[] {
                new PadSize(new Measurement(1.0f, MeasurementUnit.Inches)),
                new PadSize(new Measurement(2.0f, MeasurementUnit.Inches))
            }.ToList();

            var updates = new PadSizeCreateOrUpdate[] {
                new PadSizeCreateOrUpdate(existing[0].Id, new Measurement(1.0f, MeasurementUnit.Inches))
            }.ToList();

            var updated = h.UpdatePadSizes(existing, updates);
            Assert.AreEqual(1, updated.Count);
            Assert.AreEqual(existing[0].Id, updated[0].Id);
        }

        [TestMethod]
        public void UpdatePadColorsExistingColorsAreUpdated() {
            var h = new PadSeriesUpdateHandler(
                            Mock.Of<IPadSeriesRepo>(),
                            new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                            Mock.Of<IImageProcessor>()
            );

            var existing = new Pad[] {
                new Pad("ColorA", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red,  false,null, new PadOption[] {
                    new PadOption(Guid.NewGuid()),
                }.ToList())
            }.ToList();

            var updates = new PadCreateOrUpdate[]{
                new PadCreateOrUpdate(existing[0].Id, "ColorAA", PadCategory.Finishing, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red,  false,null!, new PadOptionCreateOrUpdate[] {
                    new PadOptionCreateOrUpdate(Guid.NewGuid())
                }.ToList())
                }.ToList();

            var updated = h.UpdatePadColors(existing, updates);
            Assert.AreEqual(1, updated.Count);

            Assert.AreEqual(existing[0].Id, updated[0].Id);
            Assert.AreEqual("ColorAA", updated[0].Name);
            Assert.AreEqual(PadCategory.Finishing, updated[0].Category);
            Assert.AreEqual(1, updated[0].Options.Count);
        }

        [TestMethod]
        public void UpdatePadColorsAddsNewColors() {
            var h = new PadSeriesUpdateHandler(
                                        Mock.Of<IPadSeriesRepo>(),
                                        new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                                        Mock.Of<IImageProcessor>()
                        );

            List<Pad> existing = new();

            var updates = new PadCreateOrUpdate[]{
                new PadCreateOrUpdate(null, "ColorB", PadCategory.Finishing, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red,  false,null!, new PadOptionCreateOrUpdate[] {
                    new PadOptionCreateOrUpdate(Guid.NewGuid())
                }.ToList())
                }.ToList();

            var updated = h.UpdatePadColors(existing, updates);
            Assert.AreEqual(1, updated.Count);
            Assert.AreEqual("ColorB", updated[0].Name);
            Assert.AreEqual(PadCategory.Finishing, updated[0].Category);
        }

        [TestMethod]
        public void UpdatePadColorsDeletesAsNeeded() {
            var h = new PadSeriesUpdateHandler(
                                        Mock.Of<IPadSeriesRepo>(),
                                        new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                                        Mock.Of<IImageProcessor>()
                        );

            var existing = new Pad[] {
                new Pad("ColorA", PadCategory.Cutting, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false,null, new PadOption[] {
                    new PadOption(Guid.NewGuid()),
                }.ToList())
            }.ToList();

            var updated = h.UpdatePadColors(existing, new());
            Assert.AreEqual(0, updated.Count);
        }

        [TestMethod]
        public void UpdatePadColorsOrdersByName() {
            var h = new PadSeriesUpdateHandler(
                                        Mock.Of<IPadSeriesRepo>(),
                                        new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                                        Mock.Of<IImageProcessor>()
                        );

            List<Pad> existing = new();

            var updates = new PadCreateOrUpdate[]{
                new PadCreateOrUpdate(null, "B", PadCategory.Finishing, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red,  false,null!, new PadOptionCreateOrUpdate[] {
                    new PadOptionCreateOrUpdate(Guid.NewGuid())
                }.ToList()),
                 new PadCreateOrUpdate(null, "A", PadCategory.Finishing, PadMaterial.Foam, PadTexture.Dimpled, PadColor.Red, false, null!, new PadOptionCreateOrUpdate[] {
                    new PadOptionCreateOrUpdate(Guid.NewGuid())
                }.ToList())
                }.ToList();

            var updated = h.UpdatePadColors(existing, updates);
            Assert.AreEqual("A", updated[0].Name);
            Assert.AreEqual("B", updated[1].Name);
        }

        [TestMethod]
        public void OrdersPadSizesByDiameter() {
            var h = new PadSeriesUpdateHandler(
                            Mock.Of<IPadSeriesRepo>(),
                            new Mock<PadSeriesCreateOrUpdateCompositeSpecification>(null, null, null, null, null).Object,
                            Mock.Of<IImageProcessor>()
                        );

            var existing = new PadSize[] {
                new PadSize(new Measurement(1.0f, MeasurementUnit.Inches)),
            }.ToList();

            var news = new PadSizeCreateOrUpdate[] {
                new PadSizeCreateOrUpdate(existing[0].Id, new Measurement(1.0f, MeasurementUnit.Inches), new Measurement(2.5f, MeasurementUnit.Inches)),
                new PadSizeCreateOrUpdate(null, new Measurement(4.0f, MeasurementUnit.Inches))
            }.ToList();

            var updated = h.UpdatePadSizes(existing, news);
            Assert.AreEqual(4f, updated[0].Diameter.Amount);
            Assert.AreEqual(1f, updated[1].Diameter.Amount);
        }
    }
}