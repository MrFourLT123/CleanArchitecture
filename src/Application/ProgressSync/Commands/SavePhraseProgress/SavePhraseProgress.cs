using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.ProgressSync.Commands.SavePhraseProgress;

public record SavePhraseProgressCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public int PhraseId { get; set; }
    public string PhraseText { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public int Score { get; set; }
}

public class SavePhraseProgressCommandHandler : IRequestHandler<SavePhraseProgressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public SavePhraseProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(SavePhraseProgressCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.PhraseProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.PhraseId == request.PhraseId, cancellationToken);

        if (existing is null)
        {
            var entity = new PhraseProgress
            {
                UserId = request.UserId,
                PhraseId = request.PhraseId,
                PhraseText = request.PhraseText,
                GroupId = request.GroupId,
                AttemptCount = 1,
                BestScore = request.Score,
                LastScore = request.Score,
                IsMastered = request.Score >= 85,
                FirstAttemptAt = DateTime.UtcNow,
                LastAttemptAt = DateTime.UtcNow
            };
            _context.PhraseProgresses.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        existing.AttemptCount++;
        existing.LastScore = request.Score;
        existing.BestScore = Math.Max(existing.BestScore, request.Score);
        // Mastered when score >= 85 on 2+ attempts
        existing.IsMastered = existing.BestScore >= 85 && existing.AttemptCount >= 2;
        existing.LastAttemptAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return existing.Id;
    }
}
