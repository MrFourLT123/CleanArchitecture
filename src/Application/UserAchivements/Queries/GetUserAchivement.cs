using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.UserAchivements.Queries.GetuserAchivement;

public record GetUserAchivementQuery : IRequest<List<UserAchivement>>;

public class GetUserAchivementHandler : IRequestHandler<GetUserAchivementQuery, List<UserAchivement>>
{
    private readonly IApplicationDbContext _context;

    public GetUserAchivementHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserAchivement>> Handle(GetUserAchivementQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.UserAchivements.ToListAsync(cancellationToken);
        return list;
    }
}
