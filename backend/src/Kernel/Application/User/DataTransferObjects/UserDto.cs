using System;

public class UserDto {
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Name { get; set; }
}