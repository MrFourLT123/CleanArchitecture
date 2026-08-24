namespace CleanArchitecture.Application.Common.Models;

public class UserDetailsDto
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? AvatarUrl { get; set; }
}
