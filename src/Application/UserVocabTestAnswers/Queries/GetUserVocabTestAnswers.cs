using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.UserVocabTestAnswers.Queries.GetuserVocabTestAnswers;

public record GetUserVocabTestAnswerQuery : IRequest<List<UserVocabTestAnswer>>;

public class GetUserVocabTestAnswerQueryHandler : IRequestHandler<GetUserVocabTestAnswerQuery, List<UserVocabTestAnswer>>
{
    private readonly IApplicationDbContext _context;

    public GetUserVocabTestAnswerQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserVocabTestAnswer>> Handle(GetUserVocabTestAnswerQuery request, CancellationToken cancellationToken)
    {
        var userVocabTestAnswers = await _context.UserVocabTestAnswers.ToListAsync(cancellationToken);
        return userVocabTestAnswers;
    }
}