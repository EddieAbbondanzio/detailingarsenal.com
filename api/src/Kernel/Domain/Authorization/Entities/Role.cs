
using System.Collections.Generic;

public class Role : Entity<Role> {
    public string Name { get; set; } = null!;
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}