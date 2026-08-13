using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Excercises.Queries.GetExcercises;

public class ExcerciseDto
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public required string Question { get; set; }
    public required string Type { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Excercise, ExcerciseDto>();
        }
    }
}
