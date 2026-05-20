using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class VocabCard
{
    public int Id { get; set; }
    public required string Word { get; set; }
    public string? Definition { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string? PartOfSpeech { get; set; }
    public string? Ipa { get; set; }
    public string? Synonyms { get; set; }
    public string? Level { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
