public class CustomerInfo : Entity<CustomerInfo> {
    public string ExternalId { get; }
    public string Email { get; }

    public CustomerInfo(string externalId, string email) {
        ExternalId = externalId;
        Email = email;
    }
}