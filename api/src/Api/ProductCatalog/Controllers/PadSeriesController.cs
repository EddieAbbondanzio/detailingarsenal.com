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
                    create.Sizes.Select(s => new PadSeriesSize(s.Diameter, s.Thickness, s.PartNumber)).ToList(),
                    create.Pads.Select(p => p.ToReal()).ToList()
                ),
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
                    update.Sizes,
                    update.Pads.Select(p => p.ToReal()).ToList()
                ),
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