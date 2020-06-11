
namespace DetailingArsenal.Domain {
    public class NewUserEvent : IBusEvent {
        public User User { get; }

        public NewUserEvent(User user) {
            User = user;
        }
    }
}