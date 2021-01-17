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
using System.Drawing;
using System.IO;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Api.ProductCatalog {
    [Authorize]
    [ApiController]
    [Route("product-catalog/pad-series")]
    public class PadSeriesController : ControllerBase {
        IMediator mediator;

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
        public async Task<IActionResult> Create(PadSeriesCreateCommand create) {
            var id = await mediator.Dispatch<PadSeriesCreateCommand, Guid>(create, User);

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id));

            return Ok(ps);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PadSeriesUpdateCommand update) {
            await mediator.Dispatch<PadSeriesUpdateCommand>(update, User);

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id));

            return Ok(ps);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Dispatch<PadSeriesDeleteCommand>(new(id), User);
            return Ok();
        }
    }
}