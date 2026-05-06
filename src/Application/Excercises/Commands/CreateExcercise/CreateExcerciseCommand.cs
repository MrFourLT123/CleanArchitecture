using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Excercises.Commands.CreateExcercise;

public record CreateExcerciseCommand(int LessonId, string Question, string Type) : IRequest<int>;
