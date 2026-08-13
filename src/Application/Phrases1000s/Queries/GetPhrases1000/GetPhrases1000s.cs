using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Phrases1000s.Queries;

public record GetPhrases1000sQuery : IRequest<PaginatedList<Phrases1000Dto>>
{
    public int GroupId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPhrases1000sQueryHandler : IRequestHandler<GetPhrases1000sQuery, PaginatedList<Phrases1000Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(10);

    public GetPhrases1000sQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = cache;
    }

    public async Task<PaginatedList<Phrases1000Dto>> Handle(GetPhrases1000sQuery request, CancellationToken cancellationToken)
    {
        try
        {
            string cacheKey = $"Phrases_group_{request.GroupId}";
            if (!_memoryCache.TryGetValue(cacheKey, out List<Phrases1000Dto>? cachedData) || cachedData is null)
            {
                cachedData = await _context.Phrases1000
                    .Where(p => p.GroupId == request.GroupId)
                    .ProjectTo<Phrases1000Dto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheDuration)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.Normal);

                _memoryCache.Set(cacheKey, cachedData, cacheEntryOptions);
            }

            var count = cachedData.Count;
            var items = cachedData
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginatedList<Phrases1000Dto>(items, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving Phrases1000s.", ex);
        }
    }
}
