using System;

namespace DetailingArsenal.Api.Users.Security {
    public class PermissionUpdateRequest : IDataTransferObject {
        public Guid Id { get; set; }
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}