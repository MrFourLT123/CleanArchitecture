namespace CleanArchitecture.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime Expiration) GenerateToken(string userId, string userName, string email, IList<string> roles);
}
