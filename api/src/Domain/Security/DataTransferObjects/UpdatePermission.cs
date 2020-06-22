namespace DetailingArsenal.Domain.Security {
    public class UpdatePermission : IDataTransferObject {
        public string Action { get; }
        public string Scope { get; }

        public UpdatePermission(string action, string scope) {
            Action = action;
            Scope = scope;
        }
    }
}