using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Achievements.Queries;

public record GetAchievementsQuery : IRequest<List<Achievement>>;

public class GetAchievementsQueryHandler : IRequestHandler<GetAchievementsQuery, List<Achievement>>
{
    private readonly IApplicationDbContext _context;
    public GetAchievementsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Achievement>> Handle(GetAchievementsQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Achievements.ToListAsync(cancellationToken);
        return list;
    }
}
