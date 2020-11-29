using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.Users {
    public record UserReadModel(string Email, string Username, string? Name, DateTime JoinedDate, bool IsAdmin, List<UserPermissionReadModel> Permissions) : IDataTransferObject;
}