using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabCards.Commands.UpdateVocabCard;

public record UpdateVocabCardCommand : IRequest
{
    public int Id { get; set; }
    public required string Word { get; set; }
    public string? Definition { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateVocabCardCommandHandler : IRequestHandler<UpdateVocabCardCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateVocabCardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateVocabCardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VocabCards.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.Word = request.Word;
        entity.Definition = request.Definition;
        entity.ExampleSentence = request.ExampleSentence;
        entity.ImageUrl = request.ImageUrl;
        entity.AudioUrl = request.AudioUrl;
        entity.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}