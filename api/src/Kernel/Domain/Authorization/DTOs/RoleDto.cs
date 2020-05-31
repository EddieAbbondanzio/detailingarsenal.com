using System;
using System.Collections.Generic;

public class RoleDto : IDataTransferObject {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Guid> PermissionIds { get; set; } = new List<Guid>();
}