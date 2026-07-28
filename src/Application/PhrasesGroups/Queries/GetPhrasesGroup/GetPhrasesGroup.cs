using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.PhrasesGroups.Queries;

public record GetPhrasesGroupQuery : IRequest<List<PhrasesGroup>>;

public class GetPhrasesGroupQueryHandler : IRequestHandler<GetPhrasesGroupQuery, List<PhrasesGroup>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    private const string CACHE_KEY = "PhrasesGroup_All";
    private static readonly TimeSpan CacheDuratioin = TimeSpan.FromDays(10);

    public GetPhrasesGroupQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = cache;
    }

    public async Task<List<PhrasesGroup>> Handle(GetPhrasesGroupQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (_memoryCache.TryGetValue(CACHE_KEY, out List<PhrasesGroup>? cachedData) && cachedData is not null)
            {
                return cachedData;
            }

            var listPhrasesGroup = await _context.PhrasesGroup.ToListAsync(cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuratioin)
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(CACHE_KEY, listPhrasesGroup, cacheEntryOptions);

            return listPhrasesGroup;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving FunStories.", ex);
        }
    }
}
