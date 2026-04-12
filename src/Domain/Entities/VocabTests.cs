using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class VocabTests
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
}
