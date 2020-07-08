namespace DetailingArsenal.Domain.Security {
    public class PermissionUpdate : IDataTransferObject {
        public string Action { get; }
        public string Scope { get; }

        public PermissionUpdate(string action, string scope) {
            Action = action;
            Scope = scope;
        }
    }
}