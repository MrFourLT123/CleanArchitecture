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
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private const string CACHE_KEY = "Leaderboard_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(10);

    public GetLeaderboardQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<PaginatedList<LeaderboardDto>> Handle(GetLeaderboardsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!_memoryCache.TryGetValue(CACHE_KEY, out List<LeaderboardDto>? cachedData) || cachedData is null)
            {
                cachedData = await _context.Leaderboards
                    .ProjectTo<LeaderboardDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheDuration)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.Normal);

                _memoryCache.Set(CACHE_KEY, cachedData, cacheEntryOptions);
            }

            var count = cachedData.Count;
            var items = cachedData
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginatedList<LeaderboardDto>(items, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while retrieving Leaderboards.", ex);
        }
    }
}

