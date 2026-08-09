
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.FunStories.Queries;

public record GetFunStoriesQuery : IRequest<PaginatedList<FunStoriesDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFunStoriesQueryHandler : IRequestHandler<GetFunStoriesQuery, PaginatedList<FunStoriesDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    private const string CACHE_KEY = "FunStories_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(10);

    public GetFunStoriesQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PaginatedList<FunStoriesDTO>> Handle(GetFunStoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!_cache.TryGetValue(CACHE_KEY, out List<FunStoriesDTO>? cachedStories) || cachedStories is null)
            {
                cachedStories = await _context.FunStories
                    .ProjectTo<FunStoriesDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheDuration)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set(CACHE_KEY, cachedStories, cacheEntryOptions);
            }

            var count = cachedStories.Count;
            var items = cachedStories
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginatedList<FunStoriesDTO>(items, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving FunStories.", ex);
        }
    }
}

