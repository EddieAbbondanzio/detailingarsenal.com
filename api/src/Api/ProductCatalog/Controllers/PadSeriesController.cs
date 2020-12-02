using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DetailingArsenal.Application.Settings;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Application.ProductCatalog;

namespace DetailingArsenal.Api.ProductCatalog {
    [Authorize]
    [ApiController]
    [Route("product-catalog/pad-series")]
    public class PadSeriesController : ControllerBase {
        private IMediator mediator;

        public PadSeriesController(IMediator mediator) {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            List<PadSeriesReadModel> pads = await mediator.Dispatch<GetAllPadSeriesQuery, List<PadSeriesReadModel>>();
            return Ok(pads);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PadSeriesCreateRequest create) {
            var id = await mediator.Dispatch<PadSeriesCreateCommand, Guid>(
                new PadSeriesCreateCommand(
                    create.Name,
                    create.BrandId,
                    PadTextureUtils.Parse(create.Texture),
                    PadMaterialUtils.Parse(create.Material),
                    create.PolisherTypes.Select(pt => PolisherTypeUtils.Parse(pt)).ToList(),
                    create.Sizes.Select(s => new PadSize(s.Diameter, s.Thickness)).ToList(),
                    create.Colors.Select(
                        c => new PadColor(
                            c.Name,
                            PadCategoryUtils.Parse(c.Category),
                            c.Image,
                            c.Options.Select(o => new PadOption(o.PadSizeId, o.PartNumber)).ToList()
                        )
                    ).ToList()),
                User
            );

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id));

            return Ok(ps);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PadSeriesUpdateRequest update) {
            await mediator.Dispatch<PadSeriesUpdateCommand>(
                new PadSeriesUpdateCommand(
                    id,
                    update.Name,
                    update.BrandId,
                    PadTextureUtils.Parse(update.Texture),
                    PadMaterialUtils.Parse(update.Material),
                    update.PolisherTypes.Select(pt => PolisherTypeUtils.Parse(pt)).ToList(),
                    update.Sizes.Select(s => new PadSize(s.Diameter, s.Thickness)).ToList(),
                    update.Colors.Select(
                        c => new PadColor(
                            c.Name,
                            PadCategoryUtils.Parse(c.Category),
                            c.Image,
                            c.Options.Select(o => new PadOption(o.PadSizeId, o.PartNumber)).ToList()
                        )
                    ).ToList()),
                User
            );

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id));

            return Ok(ps);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            await mediator.Dispatch<PadSeriesDeleteCommand>(new(id), User);
            return Ok();
        }
    }
}