namespace DetailingArsenal.Application.Security {
    public class CreatePermissionCommand : IAction {
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}