using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.LeaderBoards.Commands.DeleteLeaderboard;

public record DeleteLeaderboardCommand(int Id) : IRequest;

public class DeleteLeaderboardCommandHandler : IRequestHandler<DeleteLeaderboardCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteLeaderboardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Leaderboards.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.Leaderboards.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}