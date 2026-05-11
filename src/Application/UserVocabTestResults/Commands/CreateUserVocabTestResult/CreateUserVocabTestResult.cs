using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.UserVocabTestResults.Commands.CreateUserVocabTestResult;

public record CreateUserVocabTestResultCommand : IRequest<int>
{
    public required string UserId { get; set; }
    public int VocabTestId { get; set; }
    public int Score { get; set; }
}

public class CreateUserVocabTestResultCommandHandler : IRequestHandler<CreateUserVocabTestResultCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateUserVocabTestResultCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateUserVocabTestResultCommand request, CancellationToken cancellationToken)
    {
        var entity = new UserVocabTestResult
        {
            UserId = request.UserId,
            VocabTestId = request.VocabTestId,
            Score = request.Score,
            TakenAt = DateTime.UtcNow
        };

        _context.UserVocabTestResults.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}