using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class Leaderboard
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public float Points { get; set; }
    public int Rank { get; set; }
}
