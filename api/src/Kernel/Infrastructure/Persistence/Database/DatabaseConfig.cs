
/// <summary>
/// Information used to connect to a database.
/// </summary>
public abstract class DatabaseConfig : IDatabaseConfig {
    #region Properties
    /// <summary>
    /// The IP address.
    /// </summary>
    /// <value></value>
    public string Host { get; set; } = "";

    /// <summary>
    /// The port number of the IP.
    /// </summary>
    public int Port { get; set; } = 0;

    /// <summary>
    /// The username to authenticate under.
    /// </summary>
    public string User { get; set; } = "";

    /// <summary>
    /// The secret password for authentication.
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// The name of the database.
    /// </summary>
    public string Database { get; set; } = "";
    #endregion

    #region Publics
    public abstract string GetConnectionString();
    #endregion
}