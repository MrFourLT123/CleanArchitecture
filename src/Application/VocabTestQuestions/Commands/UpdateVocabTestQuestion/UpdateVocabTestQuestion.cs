using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabTestQuestions.Commands.UpdateVocabTestQuestion;

public record UpdateVocabTestQuestionCommand : IRequest
{
    public int Id { get; set; }
    public int VocabTestId { get; set; }
    public int VocabCardId { get; set; }
    public string? QuestionTextId { get; set; }
    public string? CorrectOption { get; set; }
}

public class UpdateVocabTestQuestionCommandHandler : IRequestHandler<UpdateVocabTestQuestionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateVocabTestQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateVocabTestQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VocabTestQuestions.FindAsync(new[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.VocabTestId = request.VocabTestId;
        entity.VocabCardId = request.VocabCardId;
        entity.QuestionText = request.QuestionTextId;
        entity.CorrectOption = request.CorrectOption;

        await _context.SaveChangesAsync(cancellationToken);
    }
}