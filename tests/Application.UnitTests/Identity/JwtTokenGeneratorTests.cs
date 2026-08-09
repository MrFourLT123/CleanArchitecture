using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Shouldly;

namespace CleanArchitecture.Application.UnitTests.Identity;

[TestFixture]
public class JwtTokenGeneratorTests
{
    private JwtSettings _jwtSettings = null!;
    private JwtTokenGenerator _tokenGenerator = null!;

    [SetUp]
    public void SetUp()
    {
        _jwtSettings = new JwtSettings
        {
            Secret = "SuperSecretJwtKeyForCleanArchitectureUnitTests123!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryInMinutes = 30
        };

        var options = Options.Create(_jwtSettings);
        _tokenGenerator = new JwtTokenGenerator(options);
    }

    [Test]
    public void GenerateToken_ShouldReturnValidJwtTokenWithExpectedClaims()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "testuser@example.com";
        var roles = new List<string> { "Administrator", "User" };

        // Act
        var (token, expiration) = _tokenGenerator.GenerateToken(userId, userName, email, roles);

        // Assert
        token.ShouldNotBeNullOrWhiteSpace();
        expiration.ShouldBeGreaterThan(DateTime.UtcNow);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Issuer.ShouldBe(_jwtSettings.Issuer);
        jwtToken.Audiences.ShouldContain(_jwtSettings.Audience);

        var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
        subClaim.ShouldBe(userId);

        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName || c.Type == ClaimTypes.Name || c.Type == "unique_name" || c.Type == "name")?.Value;
        nameClaim.ShouldBe(userName);

        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email || c.Type == ClaimTypes.Email || c.Type == "email")?.Value;
        emailClaim.ShouldBe(email);

        var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role").Select(c => c.Value).ToList();
        roleClaims.ShouldContain("Administrator");
        roleClaims.ShouldContain("User");
    }
}
