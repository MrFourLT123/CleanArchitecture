using MediatR;

namespace CleanArchitecture.Application.Excercises.Commands.UpdateExcercise;

public record UpdateExcerciseCommand(int Id, int LessonId, string Question, string Type) : IRequest<Unit>;
