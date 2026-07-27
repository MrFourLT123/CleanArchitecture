using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.Phrases1000s.Queries;

public record GetPhrases1000sQuery : IRequest<List<Phrases1000>>;

public class GetPhrases1000sQueryHandler : IRequestHandler<GetPhrases1000sQuery, List<Phrases1000>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    private const string CACHE_KEY = "VocabCard_All";
    private static readonly TimeSpan CacheDuratioin = TimeSpan.FromDays(10);

    public GetPhrases1000sQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = cache;
    }

    public async Task<List<Phrases1000>> Handle(GetPhrases1000sQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (_memoryCache.TryGetValue(CACHE_KEY, out List<Phrases1000>? cachedData) && cachedData is not null)
            {
                return cachedData;
            }

            var listPhrases1000 = await _context.Phrases1000.ToListAsync(cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuratioin)
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(CACHE_KEY, listPhrases1000, cacheEntryOptions);

            return listPhrases1000;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving FunStories.", ex);
        }
    }
}
