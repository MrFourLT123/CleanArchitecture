using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Excercises.Queries.GetExcercises;

public record GetExcercisesQuery : IRequest<List<Excercise>>;

public class GetExcercisesQueryHandler : IRequestHandler<GetExcercisesQuery, List<Excercise>>
{
    private readonly IApplicationDbContext _context;
    public GetExcercisesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Excercise>> Handle(GetExcercisesQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Excercises.ToListAsync(cancellationToken);
        return list;
    }
}
