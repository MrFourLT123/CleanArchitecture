using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Excercise
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public required string Question { get; set; }
    public required string Type { get; set; }

}
