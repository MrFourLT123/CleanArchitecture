using CleanArchitecture.Application.ProgressSync.Commands.SaveConversationSession;
using CleanArchitecture.Application.ProgressSync.Commands.SavePhraseGroupProgress;
using CleanArchitecture.Application.ProgressSync.Commands.SavePhraseProgress;
using CleanArchitecture.Application.ProgressSync.Commands.SaveSpeakingProgress;
using CleanArchitecture.Application.ProgressSync.Commands.SaveVocabularyProgress;

namespace CleanArchitecture.Application.ProgressSync.Commands.BulkSyncProgress;

/// <summary>
/// Container for an offline queue item sent from the mobile app.
/// The "Type" field determines which sub-command to dispatch.
/// </summary>
public record SyncEventDto
{
    /// <summary>phrase_group | phrase | conversation | vocabulary | speaking</summary>
    public string Type { get; set; } = string.Empty;

    // phrase_group fields
    public string? GroupId { get; set; }
    public string? Status { get; set; }
    public int PhrasesCount { get; set; }
    public int PhrasesPracticed { get; set; }

    // phrase fields
    public int PhraseId { get; set; }
    public string? PhraseText { get; set; }

    // conversation fields
    public string? TopicId { get; set; }
    public int MessageCount { get; set; }
    public int DurationSeconds { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime StartedAt { get; set; }

    // vocabulary fields
    public int WordId { get; set; }
    public string? Word { get; set; }
    public string? Category { get; set; }
    public string? VocabularyStatus { get; set; }
    public int CurrentWordIndex { get; set; }

    // speaking / shared score field
    public int Score { get; set; }
}

public record BulkSyncProgressCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public List<SyncEventDto> Events { get; set; } = new();
}

public class BulkSyncProgressCommandHandler : IRequestHandler<BulkSyncProgressCommand, int>
{
    private readonly ISender _sender;

    public BulkSyncProgressCommandHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<int> Handle(BulkSyncProgressCommand request, CancellationToken cancellationToken)
    {
        var processed = 0;

        foreach (var ev in request.Events)
        {
            switch (ev.Type)
            {
                case "phrase_group":
                    await _sender.Send(new SavePhraseGroupProgressCommand
                    {
                        UserId = request.UserId,
                        GroupId = ev.GroupId ?? string.Empty,
                        Status = ev.Status ?? "opened",
                        PhrasesCount = ev.PhrasesCount,
                        PhrasesPracticed = ev.PhrasesPracticed,
                        BestScore = ev.Score
                    }, cancellationToken);
                    break;

                case "phrase":
                    await _sender.Send(new SavePhraseProgressCommand
                    {
                        UserId = request.UserId,
                        PhraseId = ev.PhraseId,
                        PhraseText = ev.PhraseText ?? string.Empty,
                        GroupId = ev.GroupId ?? string.Empty,
                        Score = ev.Score
                    }, cancellationToken);
                    break;

                case "conversation":
                    await _sender.Send(new SaveConversationSessionCommand
                    {
                        UserId = request.UserId,
                        TopicId = ev.TopicId ?? string.Empty,
                        MessageCount = ev.MessageCount,
                        DurationSeconds = ev.DurationSeconds,
                        IsCompleted = ev.IsCompleted,
                        StartedAt = ev.StartedAt
                    }, cancellationToken);
                    break;

                case "vocabulary":
                    await _sender.Send(new SaveVocabularyProgressCommand
                    {
                        UserId = request.UserId,
                        WordId = ev.WordId,
                        Word = ev.Word ?? string.Empty,
                        Category = ev.Category ?? string.Empty,
                        VocabularyStatus = ev.VocabularyStatus ?? "new",
                        CurrentWordIndex = ev.CurrentWordIndex
                    }, cancellationToken);
                    break;

                case "speaking":
                    await _sender.Send(new SaveSpeakingProgressCommand
                    {
                        UserId = request.UserId,
                        WordId = ev.WordId,
                        Word = ev.Word ?? string.Empty,
                        Category = ev.Category ?? string.Empty,
                        Score = ev.Score
                    }, cancellationToken);
                    break;
            }

            processed++;
        }

        return processed;
    }
}
