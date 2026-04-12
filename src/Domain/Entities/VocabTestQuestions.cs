using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class VocabTestQuestions
{
    public int Id { get; set; }
    public int VocabTestId { get; set; }
    public int VocabCardId { get; set; }
    public string? QuestionText { get; set; }
    public string? CorrectOption { get; set; }
}
