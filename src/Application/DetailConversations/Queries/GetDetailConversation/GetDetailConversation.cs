
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.DetailConversations.Queries;

public record GetDetailConversationsQuery(int ConversationId) : IRequest<List<DetailConversation>>;

public class GetDetailConversationsQueryHandler : IRequestHandler<GetDetailConversationsQuery, List<DetailConversation>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDetailConversationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DetailConversation>> Handle(GetDetailConversationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var listDetailConversation = await _context.DetailConversations
                                        .Where(dc => dc.ConversationId == request.ConversationId).ToListAsync(cancellationToken);
            return listDetailConversation;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving detail conversations.", ex);
        }
    }
}