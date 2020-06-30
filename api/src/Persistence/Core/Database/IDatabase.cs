using System;
using System.Data;
using System.Data.Common;

namespace DetailingArsenal.Persistence {
    /// <summary>
    /// Database for data persistence.
    /// </summary>
    public interface IDatabase {
        /// <summary>
        /// Get a new context with the database.
        /// </summary>
        /// <returns>The new active database context.</returns>
        DatabaseContext GetContext();
    }
}