namespace CleanArchitecture.Domain.Entities;

public class PhraseProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int PhraseId { get; set; }
    public string PhraseText { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;

    public int AttemptCount { get; set; }
    public int BestScore { get; set; }
    public int LastScore { get; set; }

    /// <summary>True when score >= 85 on at least 2 attempts</summary>
    public bool IsMastered { get; set; }

    public DateTime FirstAttemptAt { get; set; } = DateTime.UtcNow;
    public DateTime LastAttemptAt { get; set; } = DateTime.UtcNow;
}
