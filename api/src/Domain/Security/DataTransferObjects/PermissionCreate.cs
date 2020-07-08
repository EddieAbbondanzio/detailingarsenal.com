namespace DetailingArsenal.Domain.Security {
    public class PermissionCreate : IDataTransferObject {
        public string Action { get; }
        public string Scope { get; }

        public PermissionCreate(string action, string scope) {
            Action = action;
            Scope = scope;
        }
    }
}