using System.Data;
using DetailingArsenal.Domain;
using DetailingArsenal.Tests.Persistence;

namespace DetailingArsenal.Tests.Persistence {
    public class DatabaseIntegrationTests {
        internal static IDatabase Database => DatabaseManager.Database;

        internal static IDbConnection OpenConnection() => DatabaseManager.Database.OpenConnection();
    }
}