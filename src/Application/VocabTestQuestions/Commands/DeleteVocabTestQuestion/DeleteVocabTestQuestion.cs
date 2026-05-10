using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabTestQuestions.Commands.DeleteVocabTestQuestion;

public record DeleteVocabTestQuestionCommand(int Id) : IRequest;

public class DeleteVocabTestQuestionCommandHandler : IRequestHandler<DeleteVocabTestQuestionCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteVocabTestQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteVocabTestQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VocabTestQuestions.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.VocabTestQuestions.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}