using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Progresses.Commands.CreateProgress;

public record CreateProgressCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int LessonId { get; set; }
    public bool Completed { get; set; }
    public int Score { get; set; }
}

public class CreateProgressCommandHandler : IRequestHandler<CreateProgressCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateProgressCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Progress
        {
            UserId = request.UserId,
            LessonId = request.LessonId,
            Completed = request.Completed,
            Score = request.Score,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Progresses.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}