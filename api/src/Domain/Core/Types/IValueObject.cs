namespace DetailingArsenal.Domain {
    public interface IValueObject {
    }

    public abstract class ValueObject<TValue> : IValueObject where TValue : IValueObject {

    }
}