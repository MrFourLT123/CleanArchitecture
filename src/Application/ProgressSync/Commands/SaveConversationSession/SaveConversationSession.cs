using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.ProgressSync.Commands.SaveConversationSession;

public record SaveConversationSessionCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public string TopicId { get; set; } = string.Empty;
    public int MessageCount { get; set; }
    public int DurationSeconds { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
}

public class SaveConversationSessionCommandHandler : IRequestHandler<SaveConversationSessionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public SaveConversationSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(SaveConversationSessionCommand request, CancellationToken cancellationToken)
    {
        // Each conversation is always a new session row (not an upsert)
        var entity = new ConversationSession
        {
            UserId = request.UserId,
            TopicId = request.TopicId,
            MessageCount = request.MessageCount,
            DurationSeconds = request.DurationSeconds,
            IsCompleted = request.IsCompleted,
            StartedAt = request.StartedAt,
            EndedAt = DateTime.UtcNow
        };

        _context.ConversationSessions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
