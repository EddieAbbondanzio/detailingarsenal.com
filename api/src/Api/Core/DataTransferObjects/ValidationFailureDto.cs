using DetailingArsenal.Application;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Api {
    public sealed class ValidationFailureDto : IDataTransferObject {
        #region Properties
        public string Field { get; set; } = null!;
        public string Message { get; set; } = null!;
        #endregion
    }
}