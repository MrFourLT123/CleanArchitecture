using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;

public record CreateLeaderboardCommand : IRequest<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Points { get; set; }
    public int Rank { get; set; }
}

public class CreateLeaderboardsCommandHandler : IRequestHandler<CreateLeaderboardCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateLeaderboardsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Leaderboard
        {
            UserId = request.Name,
            Points = request.Points,
            Rank = request.Rank
        };
        _context.Leaderboards.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}