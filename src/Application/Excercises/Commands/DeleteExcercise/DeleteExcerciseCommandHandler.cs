using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Excercises.Commands.DeleteExcercise;

public class DeleteExcerciseCommandHandler : IRequestHandler<DeleteExcerciseCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DeleteExcerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteExcerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Excercises.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new Exception($"Excercise with Id {request.Id} not found");
        _context.Excercises.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
