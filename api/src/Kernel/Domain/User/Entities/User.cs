using System;

public class User : Entity<User> {
    public const int NameMaxLength = 64;

    public string Auth0Id { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? Name {
        get => name;
        set {
            if (value?.Length > NameMaxLength) {
                throw new ArgumentOutOfRangeException();
            }

            name = value;
        }
    }

    private string? name;
}