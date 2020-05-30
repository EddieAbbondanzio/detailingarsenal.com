using System;
using System.Collections.Generic;

public class CreateRoleCommand : IAction {
    public string Name { get; set; } = null!;
    public List<Guid> PermissionIds { get; set; } = new List<Guid>();
}