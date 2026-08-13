using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.LeaderBoards.Queries.GetLeaderboards;

public class LeaderboardDto
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public float Points { get; set; }
    public int Rank { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Leaderboard, LeaderboardDto>();
        }
    }
}
