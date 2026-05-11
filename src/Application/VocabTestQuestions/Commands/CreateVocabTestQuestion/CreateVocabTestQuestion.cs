using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;

public record CreateVocabTestQuestionCommand : IRequest<int>
{
    public int VocabTestId { get; init; }
    public int VocabCardId { get; set; }
    public string? QuestionText { get; set; }
    public string? CorrectOption { get; set; }
}

public class CreateVocabTestQuestionCommandHandler : IRequestHandler<CreateVocabTestQuestionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateVocabTestQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateVocabTestQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = new VocabTestQuestion
        {
            VocabTestId = request.VocabTestId,
            VocabCardId = request.VocabCardId,
            QuestionText = request.QuestionText,
            CorrectOption = request.CorrectOption
        };

        _context.VocabTestQuestions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}