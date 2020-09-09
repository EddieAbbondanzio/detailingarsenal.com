using System;

namespace DetailingArsenal.Application.Users.Security {
    public class PermissionView : IDataTransferObject {
        public Guid Id { get; set; }
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}