using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.ProgressSync.Commands.SaveVocabularyProgress;

public record SaveVocabularyProgressCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    /// <summary>new | learning | known</summary>
    public string VocabularyStatus { get; set; } = "new";

    public int CurrentWordIndex { get; set; }
}

public class SaveVocabularyProgressCommandHandler : IRequestHandler<SaveVocabularyProgressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public SaveVocabularyProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(SaveVocabularyProgressCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.VocabularyProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.WordId == request.WordId, cancellationToken);

        if (existing is null)
        {
            var entity = new VocabularyProgress
            {
                UserId = request.UserId,
                WordId = request.WordId,
                Word = request.Word,
                Category = request.Category,
                VocabularyStatus = request.VocabularyStatus,
                AttemptCount = 1,
                CurrentWordIndex = request.CurrentWordIndex,
                KnownAt = request.VocabularyStatus == "known" ? DateTime.UtcNow : null,
                FirstSeenAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow
            };
            _context.VocabularyProgresses.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        existing.AttemptCount++;
        existing.VocabularyStatus = request.VocabularyStatus;
        existing.CurrentWordIndex = request.CurrentWordIndex;
        existing.LastSeenAt = DateTime.UtcNow;

        // Set KnownAt only once — first time it becomes "known"
        if (request.VocabularyStatus == "known" && existing.KnownAt is null)
            existing.KnownAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return existing.Id;
    }
}
