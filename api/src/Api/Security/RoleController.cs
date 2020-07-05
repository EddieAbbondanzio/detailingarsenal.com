using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Security {
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
            List<RoleView> perms = await mediator.Dispatch<GetRolesQuery, List<RoleView>>(new GetRolesQuery(), User.GetUserId());
            return Ok(perms);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleCommand command) {
            RoleView perm = await mediator.Dispatch<CreateRoleCommand, RoleView>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleCommand command) {
            RoleView perm = await mediator.Dispatch<UpdateRoleCommand, RoleView>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Dispatch<DeleteRoleCommand>(new DeleteRoleCommand() { Id = id }, User.GetUserId());
            return Ok();
        }
    }
}