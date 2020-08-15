namespace DetailingArsenal.Domain.Settings {
    public class BusinessUpdate : IDataTransferObject {
        public string? Name { get; }
        public string? Address { get; }
        public string? Phone { get; }

        public BusinessUpdate(string? name, string? address, string? phone) {
            Name = name;
            Address = address;
            Phone = phone;
        }
    }
}