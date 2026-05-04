using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Achievement
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
