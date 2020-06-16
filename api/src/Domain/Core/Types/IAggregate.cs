namespace DetailingArsenal.Domain {
    public interface IAggregate : IEntity { }

    public abstract class Aggregate<TEntity> : Entity<TEntity> where TEntity : class, IEntity { }
}