
using System;
using System.Collections.Generic;

public class Role : Entity<Role> {
    public string Name { get; set; } = null!;
    public List<Guid> PermissionIds { get; set; } = new List<Guid>();
}