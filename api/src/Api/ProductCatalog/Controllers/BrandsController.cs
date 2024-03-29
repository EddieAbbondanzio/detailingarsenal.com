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
using DetailingArsenal.Application.Admin.ProductCatalog;

namespace DetailingArsenal.Api.ProductCatalog {
    [Authorize]
    [ApiController]
    [Route("product-catalog/brands")]
    public class BrandsController : ControllerBase {
        private IMediator mediator;

        public BrandsController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var brands = await mediator.Dispatch<GetAllBrandsQuery, List<BrandReadModel>>(User);
            return Ok(brands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateCommand create) {
            var id = await mediator.Dispatch<BrandCreateCommand, Guid>(create, User);
            var brand = await mediator.Dispatch<GetBrandByIdQuery, BrandReadModel>(new(id));

            return Ok(brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BrandUpdateCommand update) {
            await mediator.Dispatch<BrandUpdateCommand>(update, User);
            var brand = await mediator.Dispatch<GetBrandByIdQuery, BrandReadModel>(new(id));

            return Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            await mediator.Dispatch<BrandDeleteCommand>(new(id), User);
            return Ok();
        }
    }
}