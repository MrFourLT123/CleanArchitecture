using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabCards.Commands.CreateVocabCard;

public record CreateVocabCardCommand : IRequest<int>
{
    public required string Word { get; set; }
    public string? Definition { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int CategoryId { get; set; }
}

public class CreateVocabCardCommandHandler : IRequestHandler<CreateVocabCardCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateVocabCardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateVocabCardCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.VocabCard
        {
            Word = request.Word,
            Definition = request.Definition,
            ExampleSentence = request.ExampleSentence,
            ImageUrl = request.ImageUrl,
            AudioUrl = request.AudioUrl,
            CategoryId = request.CategoryId
        };
        _context.VocabCards.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
