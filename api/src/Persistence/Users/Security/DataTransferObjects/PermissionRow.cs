using System;

namespace DetailingArsenal.Persistence.Users.Security {
    internal class PermissionRow : IDataTransferObject {
        public Guid Id { get; set; }
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}