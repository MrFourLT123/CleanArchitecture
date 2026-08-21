using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.ProgressSync.Queries.GetProgressSummary;

public record ProgressSummaryDto
{
    // Phrases
    public int PhraseGroupsOpened { get; set; }
    public int PhraseGroupsMastered { get; set; }
    public int PhrasesPracticed { get; set; }
    public int PhrasesMastered { get; set; }

    // Conversations
    public int TotalConversationSessions { get; set; }
    public int TotalConversationMessages { get; set; }

    // Vocabulary
    public int VocabWordsKnown { get; set; }
    public int VocabWordsLearning { get; set; }

    // Speaking
    public int SpeakingWordsMastered { get; set; }
    public int SpeakingWordsLearning { get; set; }
    public double SpeakingAverageScore { get; set; }
}

public record GetProgressSummaryQuery(string UserId) : IRequest<ProgressSummaryDto>;

public class GetProgressSummaryQueryHandler : IRequestHandler<GetProgressSummaryQuery, ProgressSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetProgressSummaryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProgressSummaryDto> Handle(GetProgressSummaryQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var phraseGroups = await _context.PhraseGroupProgresses
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var phrases = await _context.PhraseProgresses
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var conversations = await _context.ConversationSessions
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var vocab = await _context.VocabularyProgresses
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var speaking = await _context.SpeakingProgresses
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        return new ProgressSummaryDto
        {
            PhraseGroupsOpened = phraseGroups.Count,
            PhraseGroupsMastered = phraseGroups.Count(x => x.Status == "mastered"),
            PhrasesPracticed = phrases.Count,
            PhrasesMastered = phrases.Count(x => x.IsMastered),

            TotalConversationSessions = conversations.Count,
            TotalConversationMessages = conversations.Sum(x => x.MessageCount),

            VocabWordsKnown = vocab.Count(x => x.VocabularyStatus == "known"),
            VocabWordsLearning = vocab.Count(x => x.VocabularyStatus == "learning"),

            SpeakingWordsMastered = speaking.Count(x => x.MasteryLevel == "mastered"),
            SpeakingWordsLearning = speaking.Count(x => x.MasteryLevel == "learning"),
            SpeakingAverageScore = speaking.Count > 0 ? speaking.Average(x => x.AverageScore) : 0
        };
    }
}
