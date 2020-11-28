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
        private IMediator mediator;

        public RoleController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            List<RoleReadModel> perms = await mediator.Dispatch<GetAllRolesQuery, List<RoleReadModel>>(new GetAllRolesQuery(), User.GetUserId());
            return Ok(perms);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateCommand command) {
            RoleReadModel perm = await mediator.Dispatch<RoleCreateCommand, RoleReadModel>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateCommand command) {
            RoleReadModel perm = await mediator.Dispatch<RoleUpdateCommand, RoleReadModel>(command, User.GetUserId());
            return Ok(perm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Dispatch<RoleDeleteCommand>(new(id), User.GetUserId());
            return Ok();
        }
    }
}