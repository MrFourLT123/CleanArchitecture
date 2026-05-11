using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.UserVocabTestResults.Commands.UpdateUserVocabTestResult;

public record UpdateUserVocabTestResultCommand : IRequest
{
    public int Id { get; init; }
    public required string UserId { get; init; }
    public int VocabTestId { get; set; }
    public int Score { get; init; }
}

public class UpdateUserVocabTestResultCommandHandler : IRequestHandler<UpdateUserVocabTestResultCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserVocabTestResultCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateUserVocabTestResultCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserVocabTestResults.FindAsync(new[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.UserId = request.UserId;
        entity.VocabTestId = request.VocabTestId;
        entity.Score = request.Score;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
