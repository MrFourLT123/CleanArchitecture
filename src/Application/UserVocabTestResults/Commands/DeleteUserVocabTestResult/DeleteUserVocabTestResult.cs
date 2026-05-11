using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.UserVocabTestResults.Commands.DeleteUserVocabTestResult;

public record DeleteUserVocabTestResultCommand(int Id) : IRequest;

public class DeleteUserVocabTestResultCommandHandler : IRequestHandler<DeleteUserVocabTestResultCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteUserVocabTestResultCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteUserVocabTestResultCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserVocabTestResults.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.UserVocabTestResults.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}