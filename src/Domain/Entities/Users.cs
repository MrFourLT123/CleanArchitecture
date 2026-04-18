using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Users
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Name { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
