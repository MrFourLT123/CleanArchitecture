namespace CleanArchitecture.Domain.Entities;

public class SpeakingProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public int AttemptCount { get; set; }
    public int BestScore { get; set; }
    public int LastScore { get; set; }

    /// <summary>Rolling average score across all attempts</summary>
    public int AverageScore { get; set; }

    /// <summary>new | learning | mastered</summary>
    public string MasteryLevel { get; set; } = "new";

    public DateTime FirstAttemptAt { get; set; } = DateTime.UtcNow;
    public DateTime LastAttemptAt { get; set; } = DateTime.UtcNow;
}
