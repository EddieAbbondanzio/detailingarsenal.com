namespace DetailingArsenal.Domain {
    public class ExternalCustomer : ValueObject<ExternalCustomer> {
        public string Id { get; }
        public string Email { get; }

        public ExternalCustomer(string id, string email) {
            Id = id;
            Email = email;
        }
    }
}