using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Categories
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Desscription { get; set; }
}
