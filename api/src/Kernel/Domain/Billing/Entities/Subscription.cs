using System;

public class Subscription : Entity<Subscription> {
    public string ExternalId { get; set; } = null!;
    public string Status { get; set; } = null!;
}