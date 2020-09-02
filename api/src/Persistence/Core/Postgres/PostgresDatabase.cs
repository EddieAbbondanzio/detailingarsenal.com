﻿using System;
using System.Data;
using System.Data.Common;
using Npgsql;

namespace DetailingArsenal.Persistence {
    /// <summary>
    /// A database for data persistence that runs PostgreSQL.
    /// </summary>
    public sealed class PostgresDatabase : IDatabase {
        /// <summary>
        /// The connection string for initiating new connections.
        /// </summary>
        private string connection;

        /// <summary>
        /// Create a new database.
        /// </summary>
        /// <param name="connection">The connection config.</param>
        public PostgresDatabase(IDatabaseConfig config) {
            connection = config.GetConnectionString();
        }

        public DbConnection OpenConnection() {
            var c = new NpgsqlConnection(this.connection);
            c.Open();

            return c;
        }
    }
}