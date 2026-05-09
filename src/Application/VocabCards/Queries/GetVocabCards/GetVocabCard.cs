
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.VocabCards.Queries.GetVocabCards;

public record GetVocabCardQuery : IRequest<List<VocabCard>>;

public class GetVocabCardQueryHandler : IRequestHandler<GetVocabCardQuery, List<VocabCard>>
{
    private readonly IApplicationDbContext _context;

    public GetVocabCardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VocabCard>> Handle(GetVocabCardQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.VocabCards.ToListAsync(cancellationToken);
        return list;
    }
}
