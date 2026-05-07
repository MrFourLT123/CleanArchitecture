using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.UserAchivements.Commands.UpdateUserAchivement;

public record UpdateUserAchivementCommand : IRequest<int>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AchivementId { get; set; }
    public DateTime AwardedAt { get; set; }
}

public class UpdateUserAchivementCommandHandler : IRequestHandler<UpdateUserAchivementCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserAchivementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateUserAchivementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserAchivements.FindAsync(new object[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        entity.UserId = request.UserId;
        entity.AchievementId = request.AchivementId;
        entity.AwardedAt = request.AwardedAt;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
