using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class VacabTestOptions
{
    public int Id { get; set; }
    public int VocabTestQuestionId { get; set; }
    public string? OptionLabel { get; set; }
    public string? OptionText { get; set; }

}
