namespace CleanArchitecture.Domain.Entities;

public class VocabularyProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    /// <summary>new | learning | known</summary>
    public string VocabularyStatus { get; set; } = "new";

    public int AttemptCount { get; set; }
    public int CurrentWordIndex { get; set; }

    public DateTime? KnownAt { get; set; }
    public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
    public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
}
