using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Phrases1000
{
    public int Id { get; set; }
    public required string Phrases { get; set; }
    public required string Meaning { get; set; }
    public int GroupId { get; set; }
    public required string Level { get; set; }
}
