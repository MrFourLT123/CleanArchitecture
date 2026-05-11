using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.UserVocabTestResults.Queries.GetUserVocabTestResults;

public record GetUserVocabTestResultQuery : IRequest<List<UserVocabTestResult>>;

public class GetUserVocabTestResultQueryHandler : IRequestHandler<GetUserVocabTestResultQuery, List<UserVocabTestResult>>
{
    private readonly IApplicationDbContext _context;

    public GetUserVocabTestResultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserVocabTestResult>> Handle(GetUserVocabTestResultQuery request, CancellationToken cancellationToken)
    {
        var listUserVocabTestResult = await _context.UserVocabTestResults.ToListAsync(cancellationToken);
        return listUserVocabTestResult;
    }
}