
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Lessons.Queries;

public record GetLessonsQuery : IRequest<List<Lesson>>;

public class GetLessonsQueryHandler : IRequestHandler<GetLessonsQuery, List<Lesson>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLessonsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Lesson>> Handle(GetLessonsQuery request, CancellationToken cancellationToken)
    {
        var listLesson = await _context.Lessons.ToListAsync(cancellationToken);
        return listLesson;
    }
}