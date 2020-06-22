namespace DetailingArsenal.Domain.Clients {
    public class CreateClient : IDataTransferObject {
        public string Name { get; }
        public string? Phone { get; }
        public string? Email { get; }

        public CreateClient(string name, string? phone, string? email) {
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}