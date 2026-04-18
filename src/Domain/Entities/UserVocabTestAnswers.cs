using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class UserVocabTestAnswers
{
    public int Id { get; set; }
    public int UserVocabTestResultId { get; set; }
    public int VocabTestQuestionId { get; set; }
    public string SelectedOption { get; set; } = null!;
}
