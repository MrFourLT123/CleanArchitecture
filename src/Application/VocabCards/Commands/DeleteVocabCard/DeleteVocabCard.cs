
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabCards.Commands.DeleteVocabCard;

public record DeleteVocabCardCommand(int Id) : IRequest;

public class DeleteVocabCardCommandHandler : IRequestHandler<DeleteVocabCardCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteVocabCardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteVocabCardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VocabCards.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.VocabCards.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}