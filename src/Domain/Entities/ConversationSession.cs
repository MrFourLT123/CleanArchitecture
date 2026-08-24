namespace CleanArchitecture.Domain.Entities;

public class ConversationSession
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    /// <summary>e.g. "work", "travel", "daily"</summary>
    public string TopicId { get; set; } = string.Empty;

    public int MessageCount { get; set; }
    public int DurationSeconds { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }
}
