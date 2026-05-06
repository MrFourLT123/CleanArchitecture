using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Excercises.Commands.CreateExcercise;

public class CreateExcerciseCommandHandler : IRequestHandler<CreateExcerciseCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateExcerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateExcerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Excercise
        {
            LessonId = request.LessonId,
            Question = request.Question,
            Type = request.Type
        };
        _context.Excercises.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
