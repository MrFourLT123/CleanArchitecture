using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.WordProgresses.Commands.SyncWordProgress;

public record SyncWordProgressCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
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

public class SyncWordProgressCommandHandler : IRequestHandler<SyncWordProgressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public SyncWordProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(SyncWordProgressCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.WordProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.WordId == request.WordId, cancellationToken);

        if (existing == null)
        {
            var entity = new WordProgress
            {
                UserId = request.UserId,
                WordId = request.WordId,
                Word = request.Word,
                Category = request.Category,
                VocabularyStatus = request.VocabularyStatus,
                MasteryLevel = request.MasteryLevel,
                BestPronunciationScore = request.BestPronunciationScore,
                LastScore = request.LastScore,
                AverageScore = request.AverageScore,
                PronunciationAttempts = request.PronunciationAttempts,
                VocabularyAttempts = request.VocabularyAttempts,
                TotalAttempts = request.TotalAttempts,
                FirstAttemptDate = request.FirstAttemptDate,
                LastPracticedDate = request.LastPracticedDate,
                KnownAtDate = request.KnownAtDate
            };
            _context.WordProgresses.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
        else
        {
            existing.Word = request.Word;
            existing.Category = request.Category;
            existing.VocabularyStatus = request.VocabularyStatus;
            existing.MasteryLevel = request.MasteryLevel;
            existing.BestPronunciationScore = request.BestPronunciationScore;
            existing.LastScore = request.LastScore;
            existing.AverageScore = request.AverageScore;
            existing.PronunciationAttempts = request.PronunciationAttempts;
            existing.VocabularyAttempts = request.VocabularyAttempts;
            existing.TotalAttempts = request.TotalAttempts;
            existing.FirstAttemptDate = request.FirstAttemptDate;
            existing.LastPracticedDate = request.LastPracticedDate;
            existing.KnownAtDate = request.KnownAtDate;

            await _context.SaveChangesAsync(cancellationToken);
            return existing.Id;
        }
    }
}
