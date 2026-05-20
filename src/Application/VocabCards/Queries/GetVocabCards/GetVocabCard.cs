
using Application.VocabCards.Queries.GetVocabCards;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabCards.Queries.GetVocabCards;

public record GetVocabCardQuery : IRequest<List<VocabCardDto>>;

public class GetVocabCardQueryHandler : IRequestHandler<GetVocabCardQuery, List<VocabCardDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVocabCardQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VocabCardDto>> Handle(GetVocabCardQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.VocabCards
        .Include(v => v.Category)
        .ProjectTo<VocabCardDto>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
        return list;
    }
}
