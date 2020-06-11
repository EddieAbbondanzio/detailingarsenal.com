namespace DetailingArsenal.Domain {
    public class Customer : Entity<Customer> {
        public CustomerInfo Info { get; set; } = null!;
    }
}