
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class ExceptionDto : IDataTransferObject {
        public virtual string Type { get; } = null!;
        public string Message { get; set; } = null!;
    }
}