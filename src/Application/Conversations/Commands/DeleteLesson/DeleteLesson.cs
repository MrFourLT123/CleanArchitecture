using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Lessons.Commands.DeleteLesson;

public record DeleteLessonCommand(int Id) : IRequest;

public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Lessons.FindAsync(new object[] { request.Id }, cancellationToken);
        
        Guard.Against.NotFound(request.Id, entity);

        _context.Lessons.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
