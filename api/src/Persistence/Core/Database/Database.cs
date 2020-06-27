using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace DetailingArsenal.Persistence {

    /// <summary>
    /// Base class for databases to implement.
    /// </summary>
    public abstract class Database : IDatabase {
        protected IServiceProvider serviceProvider;

        public Database(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get a new context with the database.
        /// </summary>
        /// <returns></returns>
        public DatabaseContext GetContext() {
            var connection = GetConnection();
            connection.Open();

            return new DatabaseContext(connection, serviceProvider);
        }

        protected abstract DbConnection GetConnection();
    }
}