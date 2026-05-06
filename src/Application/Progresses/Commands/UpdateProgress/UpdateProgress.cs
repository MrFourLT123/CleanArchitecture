using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Progresses.Commands.UpdateProgress;

public record UpdateProgressCommand : IRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int LessonId { get; set; }
    public bool Completed { get; set; }
    public int Score { get; set; }
}

public class UpdateProgressCommandHandler : IRequestHandler<UpdateProgressCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Progresses.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.UserId = request.UserId;
        entity.LessonId = request.LessonId;
        entity.Completed = request.Completed;
        entity.Score = request.Score;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }
}