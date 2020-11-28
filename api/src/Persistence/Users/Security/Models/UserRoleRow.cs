using System;

namespace DetailingArsenal.Persistence.Users.Security {
    internal class UserRoleRow : IDataTransferObject {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}