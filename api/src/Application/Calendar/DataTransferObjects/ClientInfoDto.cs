using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Calendar {
    public class ClientInfoDto : IDataTransferObject {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}