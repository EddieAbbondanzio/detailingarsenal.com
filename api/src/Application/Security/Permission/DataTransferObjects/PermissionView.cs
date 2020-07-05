using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Security {
    public class PermissionView : IDataTransferObject {
        public Guid Id { get; set; }
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}