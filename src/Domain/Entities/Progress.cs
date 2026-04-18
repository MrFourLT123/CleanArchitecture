using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Progress
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public int LessonId { get; set; }
    public bool Completed { get; set; }
    public int Score { get; set; }
    public DateTime UpdatedAt { get; set; }
}
