using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Lessons
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Type { get; set; }
    public string? Content { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
}
