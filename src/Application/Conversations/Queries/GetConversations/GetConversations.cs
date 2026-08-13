
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.Conversations.Queries;

public record GetConversationsQuery : IRequest<PaginatedList<ConversationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, PaginatedList<ConversationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private const string CACHE_KEY = "Conversation_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(10);

    public GetConversationsQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<PaginatedList<ConversationDto>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!_memoryCache.TryGetValue(CACHE_KEY, out List<ConversationDto>? cachedData) || cachedData is null)
            {
                cachedData = await _context.Conversations
                    .ProjectTo<ConversationDto>(_mapper.ConfigurationProvider)
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

            return new PaginatedList<ConversationDto>(items, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving Conversations.", ex);
        }
    }
}