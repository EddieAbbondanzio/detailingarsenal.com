using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "update", Scope = "pad-series")]
    public class PadSeriesUpdateHandler : ActionHandler<PadSeriesUpdateCommand> {
        IPadSeriesRepo repo;
        PadSeriesCreateOrUpdateCompositeSpecification spec;
        IImageProcessor imageProcessor;

        public PadSeriesUpdateHandler(IPadSeriesRepo repo, PadSeriesCreateOrUpdateCompositeSpecification spec, IImageProcessor imageProcessor) {
            this.repo = repo;
            this.spec = spec;
            this.imageProcessor = imageProcessor;
        }

        public async override Task Execute(PadSeriesUpdateCommand command, User? user) {
            var series = await repo.FindById(command.Id) ?? throw new EntityNotFoundException();
            var origColors = series.Colors;

            series.Name = command.Name;
            series.BrandId = command.BrandId;
            series.Material = command.Material;
            series.Texture = command.Texture;

            series.Sizes = UpdatePadSizes(series.Sizes, command.Sizes);

            // series.Colors = command.Colors.Select(c => new PadColor(
            //     c.Name,
            //     c.Category,
            //         null,
            //     c.Options.Select(o => new PadOption(
            //         o.PadSizeId,
            //         o.PartNumber

            //     )).ToList()
            //     )).ToList();

            for (int i = 0; i < series.Colors.Count; i++) {
                var color = series.Colors[i] ?? throw new NullReferenceException();
                var colorUpdate = command.Colors[i] ?? throw new NullReferenceException();

                // Don't need to worry about pad colors with no images after updating.
                if (colorUpdate.Image == null) {
                    continue;
                }

                // #pragma warning disable 8602 // Losing my mind. Compiler keeps saying possible null dereference. Unless it's really right ...
                //                 switch (colorUpdate.Image.Action) {
                //                     case PadColorImageUpdateAction.ReplaceImage:
                //                         color.Image = imageProcessor.Process(colorUpdate!.Image!.NewImage.Name, colorUpdate!.Image.NewImage.Data);
                //                         color.Image.Parent = new ImageParentReference(color.Id, ImageParentType.PadColor);
                //                         break;

                //                     case PadColorImageUpdateAction.DoNothing:
                //                         color.Image = origColors.Find(c => c.Name == colorUpdate.Name)!.Image;
                //                         break;
                //                 }
                // #pragma warning restore 8602
            }

            await spec.CheckAndThrow(series);
            await repo.Update(series);
        }

        public List<PadSize> UpdatePadSizes(List<PadSize> existing, List<PadSizeCreateOrUpdate> updates) {
            var sizes = new List<PadSize>();

            // Update any existing pad sizes
            foreach (var size in existing) {
                var update = updates.Find(s => s.Id == size.Id);

                // If no update with the pad series exists, it was deleted.
                if (update == null)
                    continue;

                size.Diameter = update.Diameter;
                size.Thickness = update.Thickness;

                // Add it to the new list.
                sizes.Add(size);
            }

            // Add any new sizes
            var newSizes = updates.Where(s => s.Id == null);
            foreach (var size in newSizes) {
                sizes.Add(new PadSize(size.Diameter, size.Thickness));
            }

            return sizes;
        }

        public List<PadColor> UpdatePadColors(List<PadColor> existing, List<PadColorCreateOrUpdate> updates) {
            var colors = new List<PadColor>();

            // Update any existing pad colors
            foreach (var color in existing) {
                var update = updates.Find(c => c.Id == color.Id);

                // If no update exists, it was deleted.
                if (update == null) {
                    continue;
                }

                color.Name = update.Name;
                color.Category = update.Category;
                // color.Image = imageProcessor.Process(update.Image!.NewImage.Name, update.Image.NewImage.Data);
            }

            throw new NotImplementedException();
        }
    }
}