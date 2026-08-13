
using Application.VocabCards.Queries.GetVocabCards;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.VocabCards.Queries.GetVocabCards;

public record GetVocabCardQuery : IRequest<PaginatedList<VocabCardDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetVocabCardQueryHandler : IRequestHandler<GetVocabCardQuery, PaginatedList<VocabCardDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    private const string CACHE_KEY = "VocabCard_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(10);

    public GetVocabCardQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = cache;
    }

    public async Task<PaginatedList<VocabCardDto>> Handle(GetVocabCardQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!_memoryCache.TryGetValue(CACHE_KEY, out List<VocabCardDto>? cachedData) || cachedData is null)
            {
                cachedData = await _context.VocabCards
                                .Include(v => v.Category)
                                .ProjectTo<VocabCardDto>(_mapper.ConfigurationProvider)
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

            return new PaginatedList<VocabCardDto>(items, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving VocabCards.", ex);
        }
    }
}
