namespace DetailingArsenal.Api.Users.Security {
    public class PermissionCreateRequest : IDataTransferObject {
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}