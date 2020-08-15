namespace DetailingArsenal.Domain.Clients {
    public class ClientUpdate : IDataTransferObject {
        public string Name { get; }
        public string? Phone { get; }
        public string? Email { get; }

        public ClientUpdate(string name, string? phone, string? email) {
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}