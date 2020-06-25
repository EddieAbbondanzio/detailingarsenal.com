using System;

namespace DetailingArsenal.Application.Users {
    public class UserDto {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
    }
}