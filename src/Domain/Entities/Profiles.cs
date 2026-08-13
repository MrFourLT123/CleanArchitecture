using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Profiles
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public int Level { get; set; }
    public int Points { get; set; }
    public DateTime LastActive { get; set; }
}
