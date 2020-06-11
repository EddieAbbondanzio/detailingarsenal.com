namespace DetailingArsenal.Application {
    public class CreatePermissionCommand : IAction {
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}