using System;
using System.Data;
using System.Data.Common;

namespace DetailingArsenal.Persistence {
    /// <summary>
    /// Database for data persistence.
    /// </summary>
    public interface IDatabase {
        DbConnection OpenConnection();
    }
}