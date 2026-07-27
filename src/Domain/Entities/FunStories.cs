using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class FunStory
{
    public int Id { get; set; }
    public required string TitleEN { get; set; }
    public required string TitleVN { get; set; }
    public required string ContentEN { get; set; }
    public required string ContentVN { get; set; }
}
