
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.FunStories.Queries;

public record GetFunStoriesQuery : IRequest<List<FunStory>>;

public class GetFunStoriesQueryHandler : IRequestHandler<GetFunStoriesQuery, List<FunStory>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    private const string CACHE_KEY = "FunStories_All";
    private static readonly TimeSpan CacheDuratioin = TimeSpan.FromDays(10);

    public GetFunStoriesQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<List<FunStory>> Handle(GetFunStoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.TryGetValue(CACHE_KEY, out List<FunStory>? cachedStories) && cachedStories is not null)
            {
                return cachedStories;
            }

            var listFunStories = await _context.FunStories.ToListAsync(cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuratioin)
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(CACHE_KEY, listFunStories, cacheEntryOptions);

            return listFunStories;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving FunStories.", ex);
        }
    }
}
