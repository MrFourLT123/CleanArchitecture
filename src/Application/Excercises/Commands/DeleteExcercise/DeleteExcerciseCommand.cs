using MediatR;

namespace CleanArchitecture.Application.Excercises.Commands.DeleteExcercise;

public record DeleteExcerciseCommand(int Id) : IRequest<Unit>;
