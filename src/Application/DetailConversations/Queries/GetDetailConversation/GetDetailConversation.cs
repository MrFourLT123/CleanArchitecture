
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.DetailConversations.Queries;

public record GetDetailConversationsQuery(int ConversationId) : IRequest<List<DetailConversation>>;

public class GetDetailConversationsQueryHandler : IRequestHandler<GetDetailConversationsQuery, List<DetailConversation>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private static readonly TimeSpan CacheDuratioin = TimeSpan.FromDays(10);

    public GetDetailConversationsQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<List<DetailConversation>> Handle(GetDetailConversationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            string cache_key = "Detail_Conversation_" + request.ConversationId;
            if (_memoryCache.TryGetValue(cache_key, out List<DetailConversation>? cachedData) && cachedData is not null)
            {
                return cachedData;
            }
            var listDetailConversation = await _context.DetailConversations
                                        .Where(dc => dc.ConversationId == request.ConversationId).ToListAsync(cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuratioin)
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(cache_key, listDetailConversation, cacheEntryOptions);

            return listDetailConversation;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving Detail Conversation.", ex);
        }
    }
}