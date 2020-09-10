namespace DetailingArsenal.Application.Users {
    public class UserPermissionReadModel : IDataTransferObject {
        public string Action { get; }
        public string Scope { get; }

        public UserPermissionReadModel(string action, string scope) {
            Action = action;
            Scope = scope;
        }
    }
}