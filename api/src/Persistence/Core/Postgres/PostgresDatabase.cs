using System;
using System.Data;
using System.Data.Common;
using Npgsql;

namespace DetailingArsenal.Persistence {
    /// <summary>
    /// A database for data persistance that runs PostgreSQL.
    /// </summary>
    public sealed class PostgresDatabase : Database {
        /// <summary>
        /// The connection string for initiating new connections.
        /// </summary>
        private string connection;

        /// <summary>
        /// Create a new database.
        /// </summary>
        /// <param name="connection">The connection config.</param>
        public PostgresDatabase(IDatabaseConfig config, IServiceProvider serviceProivder) : base(serviceProivder) {
            connection = config.GetConnectionString();
        }

        protected override DbConnection GetConnection() => new NpgsqlConnection(this.connection);
    }
}