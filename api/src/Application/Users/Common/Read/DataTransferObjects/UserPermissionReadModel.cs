namespace DetailingArsenal.Application.Users {
    public record UserPermissionReadModel(string Action, string Scope) : IDataTransferObject;
}