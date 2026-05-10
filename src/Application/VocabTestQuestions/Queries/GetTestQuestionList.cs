using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.VocabTestQuestions.Queries.GetVocabTestQuestionList;

public record GetVocabTestQuestionQuery : IRequest<List<VocabTestQuestion>>;

public class GetVocabTestQuestionQueryHandler : IRequestHandler<GetVocabTestQuestionQuery, List<VocabTestQuestion>>
{
    private readonly IApplicationDbContext _context;

    public GetVocabTestQuestionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VocabTestQuestion>> Handle(GetVocabTestQuestionQuery request, CancellationToken cancellationToken)
    {
        var listVocabTestQuestion = await _context.VocabTestQuestions.ToListAsync(cancellationToken);
        return listVocabTestQuestion;
    }
}