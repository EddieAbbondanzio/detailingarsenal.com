using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.Users {
    public class UserReadModel : IDataTransferObject {
        public string Email { get; }
        public string Username { get; }
        public string? Name { get; }
        public DateTime JoinedDate { get; }
        public bool IsAdmin { get; }
        public List<UserPermissionReadModel> Permissions { get; }

        public UserReadModel(string email, string username, string? name, DateTime joinedDate, bool isAdmin, List<UserPermissionReadModel> permissions) {
            Email = email;
            Username = username;
            Name = name;
            JoinedDate = joinedDate;
            IsAdmin = isAdmin;
            Permissions = permissions;
        }
    }
}