using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Progresses.Commands.DeleteProgress;

public record DeleteProgressCommand(int Id) : IRequest;

public class DeleteProgressCommandHandler : IRequestHandler<DeleteProgressCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteProgressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Progresses.FindAsync(new object[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.Progresses.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}