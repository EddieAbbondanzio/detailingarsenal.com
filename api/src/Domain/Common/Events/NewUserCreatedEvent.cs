using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain {
    public class NewUserCreatedEvent : IDomainEvent {
        public User User { get; }

        public NewUserCreatedEvent(User user) {
            User = user;
        }
    }
}