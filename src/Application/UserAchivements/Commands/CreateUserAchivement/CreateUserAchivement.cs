using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.UserAchivements.Commands.CreateUserAchivement;

public record CreateUserAchivementCommand : IRequest<int>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AchivementId { get; set; }
    public DateTime AwardedAt { get; set; }
}

public class CreateAchivementCommandHandler : IRequestHandler<CreateUserAchivementCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAchivementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateUserAchivementCommand request, CancellationToken cancellationToken)
    {
        var entity = new UserAchivement
        {
            UserId = request.UserId,
            AchievementId = request.AchivementId,
            AwardedAt = request.AwardedAt
        };

        _context.UserAchivements.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}