using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.LeaderBoards.Commands.UpdateLeaderboard;

public record UpdateLeaderboardCommand : IRequest
{
    public int Id { get; init; }
    public required string UserId { get; init; }
    public int Points { get; init; }
    public int Rank { get; init; }
}

public class UpdateLeaderboardCommandHandler : IRequestHandler<UpdateLeaderboardCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateLeaderboardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(UpdateLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Leaderboards.Find(request.Id);
        Guard.Against.NotFound(request.Id, entity);
        entity.UserId = request.UserId;
        entity.Points = request.Points;
        entity.Rank = request.Rank;
        return _context.SaveChangesAsync(cancellationToken);
    }
}