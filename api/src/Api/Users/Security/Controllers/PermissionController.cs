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
        IMediator mediator;

        public PermissionController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPermissions() {
            List<PermissionReadModel> perms = await mediator.Dispatch<GetAllPermissionsQuery, List<PermissionReadModel>>(User);
            return Ok(perms);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission(PermissionCreateRequest body) {
            var res = await mediator.Dispatch<PermissionCreateCommand, CommandResult>(new(body.Action, body.Scope), User);

            var perm = await mediator.Dispatch<GetPermissionByIdQuery, PermissionReadModel?>(new(res.Data.Id), User);
            return Ok(perm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] PermissionUpdateRequest body) {
            await mediator.Dispatch<PermissionUpdateCommand>(new(id, body.Action, body.Scope), User);

            var perm = await mediator.Dispatch<GetPermissionByIdQuery, PermissionReadModel?>(new(id), User);
            return Ok(perm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(Guid id) {
            await mediator.Dispatch<PermissionDeleteCommand>(new(id), User);
            return Ok();
        }
    }
}