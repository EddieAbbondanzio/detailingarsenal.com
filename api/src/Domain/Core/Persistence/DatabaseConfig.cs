namespace DetailingArsenal.Domain {
    /// <summary>
    /// Information used to connect to a database.
    /// </summary>
    public abstract class DatabaseConfig {
        /// <summary>
        /// The IP address.
        /// </summary>
        /// <value></value>
        public string Host { get; set; } = null!;

        /// <summary>
        /// The port number of the IP.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The username to authenticate under.
        /// </summary>
        public string User { get; set; } = null!;

        /// <summary>
        /// The secret password for authentication.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// The name of the database.
        /// </summary>
        public string Database { get; set; } = null!;

        public abstract string GetConnectionString();
    }
}