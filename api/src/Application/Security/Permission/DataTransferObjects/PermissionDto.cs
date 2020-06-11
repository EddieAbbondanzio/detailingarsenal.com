using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class PermissionDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string Action { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}