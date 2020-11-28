using System;

namespace DetailingArsenal.Persistence.Users.Security {
    internal class RoleRow : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}