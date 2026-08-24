using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.WordProgresses.Queries.GetWordProgress;

public record GetWordProgressQuery(string UserId) : IRequest<List<WordProgressDto>>;

public class WordProgressDto
{
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string VocabularyStatus { get; set; } = string.Empty;
    public string MasteryLevel { get; set; } = string.Empty;
    public int BestPronunciationScore { get; set; }
    public int LastScore { get; set; }
    public int AverageScore { get; set; }
    public int PronunciationAttempts { get; set; }
    public int VocabularyAttempts { get; set; }
    public int TotalAttempts { get; set; }
    public DateTime FirstAttemptDate { get; set; }
    public DateTime LastPracticedDate { get; set; }
    public DateTime? KnownAtDate { get; set; }
}

public class GetWordProgressQueryHandler : IRequestHandler<GetWordProgressQuery, List<WordProgressDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWordProgressQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WordProgressDto>> Handle(GetWordProgressQuery request, CancellationToken cancellationToken)
    {
        return await _context.WordProgresses
            .Where(x => x.UserId == request.UserId)
            .Select(x => new WordProgressDto
            {
                WordId = x.WordId,
                Word = x.Word,
                Category = x.Category,
                VocabularyStatus = x.VocabularyStatus,
                MasteryLevel = x.MasteryLevel,
                BestPronunciationScore = x.BestPronunciationScore,
                LastScore = x.LastScore,
                AverageScore = x.AverageScore,
                PronunciationAttempts = x.PronunciationAttempts,
                VocabularyAttempts = x.VocabularyAttempts,
                TotalAttempts = x.TotalAttempts,
                FirstAttemptDate = x.FirstAttemptDate,
                LastPracticedDate = x.LastPracticedDate,
                KnownAtDate = x.KnownAtDate
            })
            .ToListAsync(cancellationToken);
    }
}
