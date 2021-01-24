using System;
using System.Data;
using System.Data.Common;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence {
    public abstract class DatabaseInteractor {
        private IDatabase database;

        public DatabaseInteractor(IDatabase database) {
            this.database = database;
        }

        public IDbConnection OpenConnection() {
            return database.OpenConnection();
        }
    }
}