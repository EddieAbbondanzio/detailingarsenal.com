using Npgsql;

public sealed class PostgresDatabaseConfig : DatabaseConfig {
    public override string GetConnectionString() => new NpgsqlConnectionStringBuilder() {
        Host = Host,
        Port = Port,
        Username = User,
        Password = Password,
        Database = Database,
        SslMode = SslMode.Require
    }.ToString();
}