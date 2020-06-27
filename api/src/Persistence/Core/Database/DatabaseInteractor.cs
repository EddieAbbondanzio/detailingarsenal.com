using System;
using System.Data.Common;

namespace DetailingArsenal.Persistence {
    public abstract class DatabaseInteractor : IDisposable {
        protected DbConnection Connection => context.Connection;

        private DatabaseContext context;
        private bool disposedValue = false; // To detect redundant calls

        public DatabaseInteractor(IDatabase database) {
            this.context = database.GetContext();
        }

        protected DbTransaction BeginTransaction() {
            return this.Connection.BeginTransaction();
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    this.context.Connection.Close();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DatabaseInteractor()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}