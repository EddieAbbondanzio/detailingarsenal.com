namespace DetailingArsenal.Application.Users {
    public record UserUpdateCommand(string Name) : IAction;
}