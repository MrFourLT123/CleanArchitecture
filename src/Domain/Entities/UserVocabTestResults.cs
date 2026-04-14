using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class UserVocabTestResults
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int Score { get; set; }
    public DateTime TakenAt { get; set; }
}
