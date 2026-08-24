using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.ProgressSync.Commands.SavePhraseGroupProgress;

public record SavePhraseGroupProgressCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public string Status { get; set; } = "opened";
    public int PhrasesCount { get; set; }
    public int PhrasesPracticed { get; set; }
    public int BestScore { get; set; }
}

public class SavePhraseGroupProgressCommandHandler : IRequestHandler<SavePhraseGroupProgressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public SavePhraseGroupProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(SavePhraseGroupProgressCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.PhraseGroupProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId, cancellationToken);

        if (existing is null)
        {
            var entity = new PhraseGroupProgress
            {
                UserId = request.UserId,
                GroupId = request.GroupId,
                Status = request.Status,
                PhrasesCount = request.PhrasesCount,
                PhrasesPracticed = request.PhrasesPracticed,
                BestScore = request.BestScore,
                FirstOpenedAt = DateTime.UtcNow,
                LastPracticedAt = DateTime.UtcNow
            };
            _context.PhraseGroupProgresses.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        // Upsert — preserve best values
        existing.Status = request.Status;
        existing.PhrasesCount = request.PhrasesCount;
        existing.PhrasesPracticed = Math.Max(existing.PhrasesPracticed, request.PhrasesPracticed);
        existing.BestScore = Math.Max(existing.BestScore, request.BestScore);
        existing.LastPracticedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return existing.Id;
    }
}
