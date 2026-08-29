using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;

public record CreateLeaderboardCommand : IRequest<int>
{
    public int Id { get; init; }
    public string? UserId { get; init; }
    public string? Name { get; init; }
    public int Points { get; init; }
    public int Rank { get; init; }
}

public class CreateLeaderboardsCommandHandler : IRequestHandler<CreateLeaderboardCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly IMemoryCache _memoryCache;
    private const string CACHE_KEY = "Leaderboard_All";

    public CreateLeaderboardsCommandHandler(IApplicationDbContext context, IUser user, IMemoryCache memoryCache)
    {
        _context = context;
        _user = user;
        _memoryCache = memoryCache;
    }

    public async Task<int> Handle(CreateLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var targetUserId = !string.IsNullOrWhiteSpace(request.UserId)
            ? request.UserId
            : (!string.IsNullOrWhiteSpace(request.Name) ? request.Name : _user.Id);

        Guard.Against.NullOrWhiteSpace(targetUserId, nameof(targetUserId), "A valid UserId is required.");

        var entity = new Domain.Entities.Leaderboard
        {
            UserId = targetUserId,
            Points = request.Points,
            Rank = request.Rank
        };

        _context.Leaderboards.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate in-memory cache so newly registered users/scores appear immediately
        _memoryCache.Remove(CACHE_KEY);

        return entity.Id;
    }
}
