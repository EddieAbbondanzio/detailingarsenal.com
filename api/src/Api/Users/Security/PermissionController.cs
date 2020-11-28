using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Users.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Users.Security {
    [Authorize]
    [Route("/security/permission")]
    [ApiController]
    public class PermissionController : ControllerBase {
        private IMediator mediator;

        public PermissionController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPermissions() {
            List<PermissionReadModel> perms = await mediator.Dispatch<GetAllPermissionsQuery, List<PermissionReadModel>>(User.GetUserId());
            return Ok(perms);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission(PermissionCreateCommand command) {
            PermissionReadModel perm = await mediator.Dispatch<PermissionCreateCommand, PermissionReadModel>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] PermissionUpdateCommand command) {
            PermissionReadModel perm = await mediator.Dispatch<PermissionUpdateCommand, PermissionReadModel>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(Guid id) {
            await mediator.Dispatch<PermissionDeleteCommand>(new(id), User.GetUserId());
            return Ok();
        }
    }
}