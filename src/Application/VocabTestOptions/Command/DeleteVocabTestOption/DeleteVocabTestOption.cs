using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.VocabTestOptions.Commands.DeleteVocabTestOption;

public record DeleteVocabTestOptionCommand(int Id) : IRequest;

public class DeleteVocabTestOptionCommandHandler : IRequestHandler<DeleteVocabTestOptionCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteVocabTestOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteVocabTestOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VocabTestOptions.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.VocabTestOptions.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}