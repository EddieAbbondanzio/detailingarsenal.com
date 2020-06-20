using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DetailingArsenal.Application.Settings;

namespace DetailingArsenal.Api.Settings {
    [Authorize]
    [ApiController]
    [Route("settings/vehicle-category")]
    public class VehicleCategoryController : ControllerBase {
        private IMediator mediator;

        public VehicleCategoryController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicleCategories() {
            List<VehicleCategoryDto> categories = await mediator.Dispatch<GetVehicleCategoriesQuery, List<VehicleCategoryDto>>(
                new GetVehicleCategoriesQuery(),
                User.GetUserId()
                );

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicleCategory(CreateVehicleCategoryCommand create) {
            VehicleCategoryDto cat = await mediator.Dispatch<CreateVehicleCategoryCommand, VehicleCategoryDto>(
                create,
                User.GetUserId()
            );

            return Ok(cat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleCategory(Guid id, [FromBody] UpdateVehicleCategoryCommand update) {
            VehicleCategoryDto cat = await mediator.Dispatch<UpdateVehicleCategoryCommand, VehicleCategoryDto>(
                update,
                User!.GetUserId()
            );

            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            await mediator.Dispatch<DeleteVehicleCategoryCommand>(new DeleteVehicleCategoryCommand() {
                Id = id
            },
                User.GetUserId());

            return Ok();
        }
    }
}