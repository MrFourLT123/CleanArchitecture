using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class VocabCards
{
    public int Id { get; set; }
    public required string Word { get; set; }
    public string? Definition { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
}
