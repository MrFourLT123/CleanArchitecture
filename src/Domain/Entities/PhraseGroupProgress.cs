namespace CleanArchitecture.Domain.Entities;

public class PhraseGroupProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;

    /// <summary>opened | practised | mastered</summary>
    public string Status { get; set; } = "opened";

    public int PhrasesCount { get; set; }
    public int PhrasesPracticed { get; set; }
    public int BestScore { get; set; }

    public DateTime FirstOpenedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastPracticedAt { get; set; } = DateTime.UtcNow;
}
