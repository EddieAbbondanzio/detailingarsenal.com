namespace DetailingArsenal.Application.Users {
    public class UpdateUserCommand : IAction {
        public string Name { get; set; } = null!;
    }
}