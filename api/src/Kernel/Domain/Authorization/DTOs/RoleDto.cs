using System;
using System.Collections.Generic;

public class RoleDto : IDataTransferObject {
    public string Name { get; set; } = null!;
    public List<Guid> PermissionIds { get; set; } = new List<Guid>();
}