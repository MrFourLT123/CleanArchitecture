using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.LeaderBoards.Commands.DeleteLeaderboard;

public record DeleteLeaderboardCommand(int Id) : IRequest;

public class DeleteLeaderboardCommandHandler : IRequestHandler<DeleteLeaderboardCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private const string CACHE_KEY = "Leaderboard_All";

    public DeleteLeaderboardCommandHandler(IApplicationDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }
    public async Task Handle(DeleteLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Leaderboards.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.Leaderboards.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate in-memory cache
        _memoryCache.Remove(CACHE_KEY);
    }
}
