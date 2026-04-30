using CleanArchitecture.Domain.Entities;

public class LessonDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public string? Type { get; init; }
    public string? Content { get; init; }
    public int Order { get; init; }
    public DateTime CreatedAt { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Lesson, LessonDto>();
        }
    }
}