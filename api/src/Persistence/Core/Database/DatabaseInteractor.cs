using System;
using System.Data.Common;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence {
    public abstract class DatabaseInteractor {
        private IDatabase database;

        public DatabaseInteractor(IDatabase database) {
            this.database = database;
        }

        public DbConnection OpenConnection() {
            return database.OpenConnection();
        }
    }
}