using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Security {
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
            List<PermissionView> perms = await mediator.Dispatch<GetPermissionsQuery, List<PermissionView>>(new GetPermissionsQuery(), User.GetUserId());
            return Ok(perms);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission(CreatePermissionCommand command) {
            PermissionView perm = await mediator.Dispatch<CreatePermissionCommand, PermissionView>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionCommand command) {
            PermissionView perm = await mediator.Dispatch<UpdatePermissionCommand, PermissionView>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(Guid id) {
            await mediator.Dispatch<DeletePermissionCommand>(new DeletePermissionCommand() { Id = id }, User.GetUserId());
            return Ok();
        }
    }
}