using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.VocabTestOptions.Queries.GetVocabTestOption;

public record GetVocabTestOptionQuery : IRequest<List<VocabTestOption>>;

public class GetVocabTestOptionQueryHandler : IRequestHandler<GetVocabTestOptionQuery, List<VocabTestOption>>
{
    private readonly IApplicationDbContext _context;

    public GetVocabTestOptionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VocabTestOption>> Handle(GetVocabTestOptionQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.VocabTestOptions.ToListAsync(cancellationToken);
        return list;
    }
}