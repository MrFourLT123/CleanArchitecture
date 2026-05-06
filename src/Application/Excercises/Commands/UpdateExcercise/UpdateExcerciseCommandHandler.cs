using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Excercises.Commands.UpdateExcercise;

public class UpdateExcerciseCommandHandler : IRequestHandler<UpdateExcerciseCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public UpdateExcerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(UpdateExcerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Excercises.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new Exception($"Excercise with Id {request.Id} not found");
        entity.LessonId = request.LessonId;
        entity.Question = request.Question;
        entity.Type = request.Type;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
