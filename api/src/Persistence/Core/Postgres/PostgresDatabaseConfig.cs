using DetailingArsenal.Domain;
using Npgsql;

namespace DetailingArsenal.Persistence {
    public sealed class PostgresDatabaseConfig : DatabaseConfig {
        public override string GetConnectionString() => new NpgsqlConnectionStringBuilder() {
            Host = Host,
            Port = Port,
            Username = User,
            Password = Password,
            Database = Database,
            SslMode = Host == "localhost" ? SslMode.Disable : SslMode.Require,
            TrustServerCertificate = true
        }.ToString();
    }
}