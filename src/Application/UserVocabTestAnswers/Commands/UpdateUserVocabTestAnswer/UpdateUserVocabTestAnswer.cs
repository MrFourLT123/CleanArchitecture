using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.UserVocabTestAnswers.Commands.UpdateUserVocabTestAnswer;

public record UpdateUserVocabTestAnswerCommand : IRequest
{
    public int Id { get; set; }
    public int VocabCardId { get; init; }
    public string? QuestionText { get; init; }
    public string? CorrectOption { get; init; }
}

public class UpdateUserVocabTestAnswerCommandHandler : IRequestHandler<UpdateUserVocabTestAnswerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserVocabTestAnswerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateUserVocabTestAnswerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VocabTestQuestions.FindAsync(new[] { request.VocabCardId }, cancellationToken);
        Guard.Against.NotFound(request.VocabCardId, entity);
        entity.QuestionText = request.QuestionText;
        entity.CorrectOption = request.CorrectOption;
        await _context.SaveChangesAsync(cancellationToken);
    }
}