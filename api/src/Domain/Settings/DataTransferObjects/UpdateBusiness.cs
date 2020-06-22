namespace DetailingArsenal.Domain.Settings {
    public class UpdateBusiness : IDataTransferObject {
        public string? Name { get; }
        public string? Address { get; }
        public string? Phone { get; }

        public UpdateBusiness(string? name, string? address, string? phone) {
            Name = name;
            Address = address;
            Phone = phone;
        }
    }
}