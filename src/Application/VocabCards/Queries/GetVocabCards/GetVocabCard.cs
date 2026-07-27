
using Application.VocabCards.Queries.GetVocabCards;
using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.VocabCards.Queries.GetVocabCards;

public record GetVocabCardQuery : IRequest<List<VocabCardDto>>;

public class GetVocabCardQueryHandler : IRequestHandler<GetVocabCardQuery, List<VocabCardDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    private const string CACHE_KEY = "VocabCard_All";
    private static readonly TimeSpan CacheDuratioin = TimeSpan.FromDays(10);

    public GetVocabCardQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = cache;
    }

    public async Task<List<VocabCardDto>> Handle(GetVocabCardQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (_memoryCache.TryGetValue(CACHE_KEY, out List<VocabCardDto>? cachedData) && cachedData is not null)
            {
                return cachedData;
            }

            var listData = await _context.VocabCards
                            .Include(v => v.Category)
                            .ProjectTo<VocabCardDto>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuratioin)
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(CACHE_KEY, listData, cacheEntryOptions);

            return listData;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving FunStories.", ex);
        }
    }
}
