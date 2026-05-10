using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabTestOptions.Commands.CreateVocabTestOption;

public record CreateVocabTestOptionCommand : IRequest<int>
{
    public int VocabTestQuestionId { get; set; }
    public string? OptionLabel { get; set; }
    public string? OptionText { get; set; }
}

public class CreateVocabTestOptionCommandHandler : IRequestHandler<CreateVocabTestOptionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateVocabTestOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateVocabTestOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.VocabTestOption
        {
            VocabTestQuestionId = request.VocabTestQuestionId,
            OptionLabel = request.OptionLabel,
            OptionText = request.OptionText
        };

        _context.VocabTestOptions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
