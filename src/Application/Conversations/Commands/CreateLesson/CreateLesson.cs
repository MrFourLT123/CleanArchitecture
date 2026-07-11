using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Lessons.Commands.CreateLesson;

public record CreateLessonCommand: IRequest<int>
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Type { get; set; }
    public string? Content { get; set; }
    public int Order { get; set; }
}

public class CreateLessonCommandHandler: IRequestHandler<CreateLessonCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Lesson
        {
            Title = request.Title,
            Type = request.Type,
            Content = request.Content,
            Order = request.Order,
            CreatedAt = DateTime.UtcNow
        };
        _context.Lessons.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
