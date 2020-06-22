namespace DetailingArsenal.Domain.Security {
    public class CreatePermission : IDataTransferObject {
        public string Action { get; }
        public string Scope { get; }

        public CreatePermission(string action, string scope) {
            Action = action;
            Scope = scope;
        }
    }
}