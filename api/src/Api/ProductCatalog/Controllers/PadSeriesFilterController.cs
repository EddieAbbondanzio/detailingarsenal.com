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
    [Route("product-catalog/pad-series/filter")]
    public class PadSeriesFilterController : ControllerBase {
        IMediator mediator;

        public PadSeriesFilterController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var filter = await mediator.Dispatch<GetPadSeriesFilterQuery, PadSeriesFilterReadModel>();
            return Ok(filter);
        }
    }
}