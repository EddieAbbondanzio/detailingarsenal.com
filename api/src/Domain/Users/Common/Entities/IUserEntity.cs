using System;

namespace DetailingArsenal.Domain.Users {
    public interface IUserEntity {
        Guid UserId { get; }
    }

    public static class IUserEntityExts {
        public static bool IsOwner(this IUserEntity entity, User user) {
            return entity.UserId == user.Id;
        }
    }
}