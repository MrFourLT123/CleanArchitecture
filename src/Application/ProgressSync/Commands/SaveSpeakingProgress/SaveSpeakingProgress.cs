using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.ProgressSync.Commands.SaveSpeakingProgress;

public record SaveSpeakingProgressCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Score { get; set; }
}

public class SaveSpeakingProgressCommandHandler : IRequestHandler<SaveSpeakingProgressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public SaveSpeakingProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(SaveSpeakingProgressCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.SpeakingProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.WordId == request.WordId, cancellationToken);

        if (existing is null)
        {
            var entity = new SpeakingProgress
            {
                UserId = request.UserId,
                WordId = request.WordId,
                Word = request.Word,
                Category = request.Category,
                AttemptCount = 1,
                BestScore = request.Score,
                LastScore = request.Score,
                AverageScore = request.Score,
                MasteryLevel = request.Score >= 85 ? "mastered" : request.Score >= 50 ? "learning" : "new",
                FirstAttemptAt = DateTime.UtcNow,
                LastAttemptAt = DateTime.UtcNow
            };
            _context.SpeakingProgresses.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        // Rolling average: ((oldAvg * oldCount) + newScore) / newCount
        var newCount = existing.AttemptCount + 1;
        existing.AverageScore = (int)Math.Round(
            ((double)(existing.AverageScore * existing.AttemptCount) + request.Score) / newCount
        );
        existing.AttemptCount = newCount;
        existing.LastScore = request.Score;
        existing.BestScore = Math.Max(existing.BestScore, request.Score);
        existing.MasteryLevel = existing.BestScore >= 85 ? "mastered"
            : existing.BestScore >= 50 ? "learning" : "new";
        existing.LastAttemptAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return existing.Id;
    }
}
