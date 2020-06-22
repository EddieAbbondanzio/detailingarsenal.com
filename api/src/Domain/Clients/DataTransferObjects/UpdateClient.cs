namespace DetailingArsenal.Domain.Clients {
    public class UpdateClient : IDataTransferObject {
        public string Name { get; }
        public string? Phone { get; }
        public string? Email { get; }

        public UpdateClient(string name, string? phone, string? email) {
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}