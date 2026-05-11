using System.Windows.Input;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;

public record DeleteUserVocabTestAnswerCommand(int Id) : IRequest;

public class DeleteUserVocabTestAnswerCommandHandler : IRequestHandler<DeleteUserVocabTestAnswerCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserVocabTestAnswerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteUserVocabTestAnswerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserVocabTestAnswers.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.UserVocabTestAnswers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}