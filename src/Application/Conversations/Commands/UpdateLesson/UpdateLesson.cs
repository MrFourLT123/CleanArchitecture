using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Lessons.Commands.UpdateLesson;

public record UpdateLessonCommand: IRequest
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Type { get; set; }
    public string? Content { get; set; }
    public int Order { get; set; }
}

public class UpdateLessonCommandHandler: IRequestHandler<UpdateLessonCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Lessons.FindAsync(new [] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.Title = request.Title;
        entity.Type = request.Type;
        entity.Content = request.Content;
        entity.Order = request.Order;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
