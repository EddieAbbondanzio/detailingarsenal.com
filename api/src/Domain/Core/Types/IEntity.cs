using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Core;

namespace DetailingArsenal.Domain {
    /// <summary>
    /// Interface for domain entities to implement.
    /// </summary>
    public interface IEntity {
        /// <summary>
        /// Unique integer id of the entity.
        /// </summary>
        Guid Id { get; set; }
    }

    public abstract class Entity<TEntity> : IEntity where TEntity : class, IEntity {
        /// <summary>
        /// The unique Id of the entity.
        /// </summary>
        public Guid Id { get; set; }

        public List<IDomainEvent> DomainEvents { get; set; } = new List<IDomainEvent>();

        /// <summary>
        /// Generate a new unique ID for an entity.
        /// </summary>
        public Guid GenerateId() {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Check to see if the entity is equal to another object.
        /// </summary>
        /// <param name="obj">The other object to check.</param>
        /// <returns>True if the other object matches the entity.</returns>
        public override bool Equals(object? obj) {
            TEntity? c = obj as TEntity;

            if (c == null) {
                return false;
            }

            return Equals(c);
        }

        /// <summary>
        /// Check to see if two entitys are equivalent.
        /// </summary>
        /// <param name="c">The other entity to check.</param>
        /// <returns>True if the entitys match.</returns>
        public bool Equals(TEntity c) => c.Id == this.Id;

        /// <summary>
        /// Get a unique hashcode of the object.
        /// </summary>
        /// <returns>The unique hashcode.</returns>
        public override int GetHashCode() => Id.GetHashCode();
    }
}