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

namespace DetailingArsenal.Api.Settings {
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
            List<BrandReadModel> brands = await mediator.Dispatch<GetBrandsQuery, List<BrandReadModel>>(
                new GetBrandsQuery(),
                User.GetUserId()
                );

            return Ok(brands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandCommand create) {
            BrandReadModel brand = await mediator.Dispatch<CreateBrandCommand, BrandReadModel>(
                create,
                User.GetUserId()
            );

            return Ok(brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBrandCommand update) {
            BrandReadModel brand = await mediator.Dispatch<UpdateBrandCommand, BrandReadModel>(
                update,
                User!.GetUserId()
            );

            return Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            await mediator.Dispatch<DeleteBrandCommand>(new DeleteBrandCommand() {
                Id = id
            },
                User.GetUserId()
            );

            return Ok();
        }
    }
}