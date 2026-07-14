
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.DetailConversations.Queries;

public record GetDetailConversationsQuery : IRequest<List<DetailConversation>>;

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
        var listDetailConversation = await _context.DetailConversations.ToListAsync(cancellationToken);
        return listDetailConversation;
    }
}