using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api {
    [Authorize]
    [Route("/security/role")]
    [ApiController]
    public class RoleController : ControllerBase {
        private IMediator mediator;

        public RoleController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            List<RoleDto> perms = await mediator.Dispatch<GetRolesQuery, List<RoleDto>>(new GetRolesQuery(), User.GetUserId());
            return Ok(perms);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleCommand command) {
            RoleDto perm = await mediator.Dispatch<CreateRoleCommand, RoleDto>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleCommand command) {
            RoleDto perm = await mediator.Dispatch<UpdateRoleCommand, RoleDto>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Dispatch<DeleteRoleCommand>(new DeleteRoleCommand() { Id = id }, User.GetUserId());
            return Ok();
        }
    }
}