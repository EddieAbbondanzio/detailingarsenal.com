using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Users.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Users.Security {
    [Authorize]
    [Route("/security/role")]
    [ApiController]
    public class RoleController : ControllerBase {
        IMediator mediator;

        public RoleController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var perms = await mediator.Dispatch<GetAllRolesQuery, List<RoleReadModel>>(User);
            return Ok(perms);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest body) {
            var r = await mediator.Dispatch<RoleCreateCommand, CommandResult>(new(body.Name, body.PermissionIds), User);
            var perm = await mediator.Dispatch<GetRoleByIdQuery, RoleReadModel?>(new(r.Data.Id));
            return Ok(perm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateRequest body) {
            var r = await mediator.Dispatch<RoleUpdateCommand, CommandResult>(new(id, body.Name, body.PermissionIds), User);
            var perm = await mediator.Dispatch<GetRoleByIdQuery, RoleReadModel?>(new(id));
            return Ok(perm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Dispatch<RoleDeleteCommand, CommandResult>(new(id), User);
            return Ok();
        }
    }
}