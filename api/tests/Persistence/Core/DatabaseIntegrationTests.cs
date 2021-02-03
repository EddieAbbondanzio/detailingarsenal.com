using System.Data;
using DetailingArsenal.Domain;
using DetailingArsenal.Tests.Persistence;

namespace DetailingArsenal.Tests.Persistence {
    public class DatabaseIntegrationTests {
        internal IDatabase Database => DatabaseManager.Database;

        internal IDbConnection OpenConnection() => DatabaseManager.Database.OpenConnection();
    }
}