namespace CleanArchitecture.Application.LeaderBoards.Queries.GetLeaderboards;

public class LeaderboardDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public int Xp { get; set; }
    public int Points { get; set; }
    public int Level { get; set; } = 1;
    public int Streak { get; set; }
    public int Rank { get; set; }
    public bool IsMe { get; set; }
}
