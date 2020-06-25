using System;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain {
    public interface IUserEntity {
        Guid UserId { get; set; }
    }

    public static class IUserEntityExts {

        public static bool IsOwner(this IUserEntity entity, User user) {
            return entity.UserId == user.Id;
        }
    }
}