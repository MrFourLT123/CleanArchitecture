namespace CleanArchitecture.Domain.Entities;

public class WordProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    /// <summary>new | learning | known</summary>
    public string VocabularyStatus { get; set; } = "new";

    /// <summary>new | learning | mastered</summary>
    public string MasteryLevel { get; set; } = "new";

    public int BestPronunciationScore { get; set; }
    public int LastScore { get; set; }
    public int AverageScore { get; set; }

    public int PronunciationAttempts { get; set; }
    public int VocabularyAttempts { get; set; }
    public int TotalAttempts { get; set; }

    public DateTime FirstAttemptDate { get; set; } = DateTime.UtcNow;
    public DateTime LastPracticedDate { get; set; } = DateTime.UtcNow;
    public DateTime? KnownAtDate { get; set; }
}
