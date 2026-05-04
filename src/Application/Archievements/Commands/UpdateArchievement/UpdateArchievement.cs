using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Achievements.Commands.UpdateAchievement;

public record UpdateAchievementCommand : IRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class UpdateAchievementCommandHandler : IRequestHandler<UpdateAchievementCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateAchievementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateAchievementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Achievements.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
