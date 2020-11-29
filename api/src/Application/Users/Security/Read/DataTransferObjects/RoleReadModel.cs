using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Users.Security {
    public record RoleReadModel {
        public Guid Id { get; }
        public string Name { get; }
        public List<PermissionReadModel> Permissions { get; }

        public RoleReadModel(Guid id, string name, List<PermissionReadModel>? permissions = null) {
            Id = id;
            Name = name;
            Permissions = permissions ?? new();
        }
    }
}