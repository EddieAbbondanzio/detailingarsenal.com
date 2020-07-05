using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Users {
    public class UserReadModel : IDataTransferObject {
        public string Email { get; }
        public string? Name { get; }
        public bool IsAdmin { get; }
        public List<UserPermissionReadModel> Permissions { get; }

        public UserReadModel(string email, string? name, bool isAdmin, List<UserPermissionReadModel> permissions) {
            Email = email;
            Name = name;
            IsAdmin = isAdmin;
            Permissions = permissions;
        }
    }
}