using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Users.Security {
    public record RoleReadModel(Guid Id, string Name, List<PermissionReadModel> Permissions) : IDataTransferObject;
}