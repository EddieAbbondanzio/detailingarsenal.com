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
    [ApiController]
    [Route("product-catalog/pads")]
    public class PadsController : ControllerBase {
        IMediator mediator;

        public PadsController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilter() {
            var filter = await mediator.Dispatch<GetPadSeriesFilterQuery, PadFilterReadModel>();
            return Ok(filter);
        }

        public async Task<IActionResult> GetAll(GetAllPadsQuery query) {
            var pads = await mediator.Dispatch<GetAllPadsQuery, PagedArray<PadSummaryReadModel>>(query);
            return Ok(pads);
        }
    }
}