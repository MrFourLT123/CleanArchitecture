using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Achievements.Commands.DeleteAchievement;

public record DeleteAchievementCommand(int Id) : IRequest;

public class DeleteAchievementCommandHandler : IRequestHandler<DeleteAchievementCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteAchievementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteAchievementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Achievements.FindAsync(new object[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.Achievements.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
