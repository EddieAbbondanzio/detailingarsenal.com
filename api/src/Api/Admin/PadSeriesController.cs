using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Application.Admin.ProductCatalog;

namespace DetailingArsenal.Api.Admin.ProductCatalog {
    [Authorize]
    [ApiController]
    [Route("admin/product-catalog/pad-series")]
    public class PadSeriesController : ControllerBase {
        IMediator mediator;

        public PadSeriesController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            var s = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id), User);
            return Ok(s);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize) {
            var pads = await mediator.Dispatch<GetAllPadSeriesQuery, PagedCollection<PadSeriesReadModel>>(
                new GetAllPadSeriesQuery(new PagingOptions(pageNumber, pageSize)),
                User
            );
            return Ok(pads);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PadSeriesCreateCommand create) {
            var id = await mediator.Dispatch<PadSeriesCreateCommand, Guid>(create, User);

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id), User);

            return Ok(ps);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PadSeriesUpdateCommand update) {
            await mediator.Dispatch<PadSeriesUpdateCommand>(update, User);

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id), User);

            return Ok(ps);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Dispatch<PadSeriesDeleteCommand>(new(id), User);
            return Ok();
        }
    }
}