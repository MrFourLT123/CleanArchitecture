
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunStories.Queries;

public record GetFunStoriesQuery : IRequest<List<FunStory>>;

public class GetFunStoriesQueryHandler : IRequestHandler<GetFunStoriesQuery, List<FunStory>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFunStoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FunStory>> Handle(GetFunStoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var listFunStories = await _context.FunStories.ToListAsync(cancellationToken);
            return listFunStories;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving FunStories.", ex);
        }
    }
}
