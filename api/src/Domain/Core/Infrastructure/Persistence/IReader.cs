namespace DetailingArsenal.Domain {
    public interface IReader { }

    /// <summary>
    /// Read only interface for reading value objects from the database.
    /// </summary>
    public interface IReader<TDataTransferObject> : IReader where TDataTransferObject : class, IDataTransferObject { }
}