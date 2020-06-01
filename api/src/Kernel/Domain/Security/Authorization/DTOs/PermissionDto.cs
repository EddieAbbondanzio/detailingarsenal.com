using System;

public class PermissionDto : IDataTransferObject {
    public Guid Id { get; set; }
    public string Action { get; set; } = null!;
    public string Scope { get; set; } = null!;
}