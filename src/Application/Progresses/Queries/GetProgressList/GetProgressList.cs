using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Progresses.Queries;

public record GetProgressListQuery : IRequest<List<Progress>>;

public class GetProgressListQueryHandler : IRequestHandler<GetProgressListQuery, List<Progress>>
{
    private readonly IApplicationDbContext _context;
    public GetProgressListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Progress>> Handle(GetProgressListQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Progresses.ToListAsync(cancellationToken);
        return list;
    }
}