namespace DetailingArsenal.Application.Users.Security {
    public record CreatePermissionCommand(string Action, string Scope) : IAction;
}