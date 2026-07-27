
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.Conversations.Queries;

public record GetConversationsQuery : IRequest<List<Conversation>>;

public class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, List<Conversation>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private const string CACHE_KEY = "Conversation_All";
    private static readonly TimeSpan CacheDuratioin = TimeSpan.FromDays(10);

    public GetConversationsQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<List<Conversation>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (_memoryCache.TryGetValue(CACHE_KEY, out List<Conversation>? cachedData) && cachedData is not null)
            {
                return cachedData;
            }

            var listConversation = await _context.Conversations.ToListAsync(cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuratioin)
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(CACHE_KEY, listConversation, cacheEntryOptions);

            return listConversation;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving FunStories.", ex);
        }
    }
}