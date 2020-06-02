using System;

public class AddRoleToUserCommand : IAction {
    public Guid RoleId { get; set; }
}