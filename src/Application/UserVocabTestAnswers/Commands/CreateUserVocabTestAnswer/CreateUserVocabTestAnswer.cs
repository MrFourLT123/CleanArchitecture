using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.UserVocabTestAnswers.Commands.CreateUserVocabTestAnswer;

public record CreateUserVocabTestAnswerCommand : IRequest<int>
{
    public int UserId { get; init; }
    public int VocabTestQuestionId { get; set; }
    public required string SelectedOption { get; set; }
}

public class CreateUserVocabTestAnswerCommandHandler : IRequestHandler<CreateUserVocabTestAnswerCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateUserVocabTestAnswerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateUserVocabTestAnswerCommand request, CancellationToken cancellationToken)
    {
        var entity = new UserVocabTestAnswer
        {
            UserVocabTestResultId = request.UserId,
            VocabTestQuestionId = request.VocabTestQuestionId,
            SelectedOption = request.SelectedOption
        };

        _context.UserVocabTestAnswers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}