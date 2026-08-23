using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.LeaderBoards.Queries.GetLeaderboards;

public record GetLeaderboardsQuery : IRequest<PaginatedList<LeaderboardDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardsQuery, PaginatedList<LeaderboardDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IUser _user;
    private readonly IMemoryCache _memoryCache;
    private const string CACHE_KEY = "Leaderboard_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public GetLeaderboardQueryHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IUser user,
        IMemoryCache memoryCache)
    {
        _context = context;
        _identityService = identityService;
        _user = user;
        _memoryCache = memoryCache;
    }

    public async Task<PaginatedList<LeaderboardDto>> Handle(GetLeaderboardsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!_memoryCache.TryGetValue(CACHE_KEY, out List<LeaderboardDto>? cachedData) || cachedData is null)
            {
                var leaderboards = await _context.Leaderboards
                    .OrderByDescending(l => l.Points)
                    .ToListAsync(cancellationToken);

                var userIds = leaderboards.Select(l => l.UserId).Distinct().ToList();

                var users = await _identityService.GetUsersAsync(userIds, cancellationToken);
                var userDict = users.ToDictionary(u => u.Id, StringComparer.OrdinalIgnoreCase);

                // Fetch activity timestamps to calculate consecutive days streak
                var sessionDates = await _context.ConversationSessions
                    .Where(x => userIds.Contains(x.UserId))
                    .Select(x => new { x.UserId, Date = x.StartedAt })
                    .ToListAsync(cancellationToken);

                var phraseDates = await _context.PhraseProgresses
                    .Where(x => userIds.Contains(x.UserId))
                    .Select(x => new { x.UserId, Date = x.LastAttemptAt })
                    .ToListAsync(cancellationToken);

                var phraseGroupDates = await _context.PhraseGroupProgresses
                    .Where(x => userIds.Contains(x.UserId))
                    .Select(x => new { x.UserId, Date = x.LastPracticedAt })
                    .ToListAsync(cancellationToken);

                var speakingDates = await _context.SpeakingProgresses
                    .Where(x => userIds.Contains(x.UserId))
                    .Select(x => new { x.UserId, Date = x.LastAttemptAt })
                    .ToListAsync(cancellationToken);

                var vocabDates = await _context.VocabularyProgresses
                    .Where(x => userIds.Contains(x.UserId))
                    .Select(x => new { x.UserId, Date = x.LastSeenAt })
                    .ToListAsync(cancellationToken);

                var allDates = sessionDates
                    .Concat(phraseDates)
                    .Concat(phraseGroupDates)
                    .Concat(speakingDates)
                    .Concat(vocabDates)
                    .GroupBy(x => x.UserId)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Date).ToList(), StringComparer.OrdinalIgnoreCase);

                cachedData = new List<LeaderboardDto>();
                int currentRank = 1;

                foreach (var lb in leaderboards)
                {
                    userDict.TryGetValue(lb.UserId, out var userDetails);
                    allDates.TryGetValue(lb.UserId, out var userActivity);

                    var name = !string.IsNullOrWhiteSpace(userDetails?.Name)
                        ? userDetails.Name
                        : (!string.IsNullOrWhiteSpace(userDetails?.UserName) ? userDetails.UserName : "User");

                    var avatar = userDetails?.AvatarUrl ?? string.Empty;
                    var streak = userActivity != null ? CalculateStreak(userActivity) : 0;
                    var level = Math.Max(1, (lb.Points / 100) + 1);

                    cachedData.Add(new LeaderboardDto
                    {
                        Id = lb.UserId,
                        UserId = lb.UserId,
                        Name = name,
                        Avatar = avatar,
                        Xp = lb.Points,
                        Points = lb.Points,
                        Level = level,
                        Streak = streak,
                        Rank = lb.Rank > 0 ? lb.Rank : currentRank
                    });

                    currentRank++;
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheDuration)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.Normal);

                _memoryCache.Set(CACHE_KEY, cachedData, cacheEntryOptions);
            }

            var currentUserId = _user.Id;
            var count = cachedData.Count;
            var items = cachedData
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(item => new LeaderboardDto
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    Name = item.Name,
                    Avatar = item.Avatar,
                    Xp = item.Xp,
                    Points = item.Points,
                    Level = item.Level,
                    Streak = item.Streak,
                    Rank = item.Rank,
                    IsMe = !string.IsNullOrEmpty(currentUserId) && string.Equals(item.UserId, currentUserId, StringComparison.OrdinalIgnoreCase)
                })
                .ToList();

            return new PaginatedList<LeaderboardDto>(items, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while retrieving Leaderboards.", ex);
        }
    }

    private static int CalculateStreak(IEnumerable<DateTime> dates)
    {
        var distinctDates = dates
            .Select(d => d.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        if (distinctDates.Count == 0)
            return 0;

        var today = DateTime.UtcNow.Date;
        var current = today;

        if (distinctDates[0] == today)
        {
            current = today;
        }
        else if (distinctDates[0] == today.AddDays(-1))
        {
            current = today.AddDays(-1);
        }
        else
        {
            return 0;
        }

        int streak = 0;
        foreach (var date in distinctDates)
        {
            if (date == current)
            {
                streak++;
                current = current.AddDays(-1);
            }
            else if (date < current)
            {
                break;
            }
        }

        return streak;
    }
}
