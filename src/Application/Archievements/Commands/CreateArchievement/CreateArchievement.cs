using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Achievements.Commands.CreateAchievement;

public record CreateAchievementCommand : IRequest<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class CreateAchievementCommandHandler : IRequestHandler<CreateAchievementCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateAchievementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateAchievementCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Achievement
        {
            Name = request.Name
        };
        _context.Achievements.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
