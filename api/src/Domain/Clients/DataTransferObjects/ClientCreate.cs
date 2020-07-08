namespace DetailingArsenal.Domain.Clients {
    public class ClientCreate : IDataTransferObject {
        public string Name { get; }
        public string? Phone { get; }
        public string? Email { get; }

        public ClientCreate(string name, string? phone, string? email) {
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}