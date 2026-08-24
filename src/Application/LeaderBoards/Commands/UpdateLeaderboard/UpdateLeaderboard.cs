using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.LeaderBoards.Commands.UpdateLeaderboard;

public record UpdateLeaderboardCommand : IRequest
{
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

    public async Task Handle(UpdateLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Leaderboards.FirstOrDefaultAsync(l => l.UserId == request.UserId, cancellationToken);
        Guard.Against.NotFound(request.UserId, entity);
        entity.Points = request.Points;
        entity.Rank = request.Rank;
        await _context.SaveChangesAsync(cancellationToken);
    }
}