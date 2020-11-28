namespace DetailingArsenal.Application.Users.Security {
    public record PermissionCreateCommand(string Action, string Scope) : IAction;
}