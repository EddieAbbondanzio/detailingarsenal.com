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
    [Route("product-catalog/brand")]
    public class BrandController : ControllerBase {
        private IMediator mediator;

        public BrandController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            List<BrandReadModel> brands = await mediator.Dispatch<GetAllBrandsQuery, List<BrandReadModel>>(User);

            return Ok(brands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateRequest create) {
            var result = await mediator.Dispatch<BrandCreateCommand, CommandResult>(new(create.Name), User);
            var brand = await mediator.Dispatch<GetBrandByIdQuery, BrandReadModel>(new(result.Data.Id));

            return Ok(brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BrandUpdateRequest update) {
            var result = await mediator.Dispatch<BrandUpdateCommand, CommandResult>(new(id, update.Name), User);
            var brand = await mediator.Dispatch<GetBrandByIdQuery, BrandReadModel>(new(id));

            return Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            var result = await mediator.Dispatch<BrandDeleteCommand, CommandResult>(new(id), User);
            return Ok();
        }
    }
}