
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Conversations.Queries;

public record GetConversationsQuery : IRequest<List<Conversation>>;

public class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, List<Conversation>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetConversationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Conversation>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var listConversation = await _context.Conversations.ToListAsync(cancellationToken);
            return listConversation;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving conversations.", ex);
        }
    }
}