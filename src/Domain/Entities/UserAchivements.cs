using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities;

public class UserAchivements
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AchievementId { get; set; }
    public DateTime AwardedAt { get; set; }
}
