using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("/authorization/permission")]
[ApiController]
public class PermissionController : ControllerBase {
    private IMediator mediator;

    public PermissionController(IMediator mediator) {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPermissions() {
        List<PermissionDto> perms = await mediator.Dispatch<GetPermissionsQuery, List<PermissionDto>>(new GetPermissionsQuery(), User.GetUserId());
        return Ok(perms);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePermission(CreatePermissionCommand command) {
        PermissionDto perm = await mediator.Dispatch<CreatePermissionCommand, PermissionDto>(command, User.GetUserId());
        return Ok(perm);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionCommand command) {
        PermissionDto perm = await mediator.Dispatch<UpdatePermissionCommand, PermissionDto>(command, User.GetUserId());
        return Ok(perm);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(Guid id) {
        await mediator.Dispatch<DeletePermissionCommand>(new DeletePermissionCommand() { Id = id }, User.GetUserId());
        return Ok();
    }
}