using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.LeaderBoards.Queries.GetLeaderboards;

public record GetLeaderboardsQuery : IRequest<List<Leaderboard>>;

public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardsQuery, List<Leaderboard>>
{
    private readonly IApplicationDbContext _context;
    public GetLeaderboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Leaderboard>> Handle(GetLeaderboardsQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Leaderboards.ToListAsync(cancellationToken);
        return list;
    }
}
