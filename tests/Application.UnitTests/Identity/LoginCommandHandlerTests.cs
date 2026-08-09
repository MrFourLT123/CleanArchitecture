using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Identity.Commands.Login;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace CleanArchitecture.Application.UnitTests.Identity;

[TestFixture]
public class LoginCommandHandlerTests
{
    private Mock<IIdentityService> _identityServiceMock = null!;
    private LoginCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _handler = new LoginCommandHandler(_identityServiceMock.Object);
    }

    [Test]
    public async Task Handle_ValidCredentials_ShouldReturnSuccessWithAuthResponse()
    {
        // Arrange
        var command = new LoginCommand
        {
            UserNameOrEmail = "user@example.com",
            Password = "Password123!"
        };

        var expectedAuthResponse = new AuthResponse
        {
            Token = "mock_jwt_token",
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = "user-123",
            UserName = "testuser",
            Email = "user@example.com",
            Roles = new List<string> { "User" }
        };

        _identityServiceMock
            .Setup(x => x.AuthenticateAsync(command.UserNameOrEmail, command.Password))
            .ReturnsAsync((Result.Success(), expectedAuthResponse));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Token.ShouldBe("mock_jwt_token");
        result.Value.UserId.ShouldBe("user-123");
    }

    [Test]
    public async Task Handle_InvalidCredentials_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new LoginCommand
        {
            UserNameOrEmail = "user@example.com",
            Password = "WrongPassword"
        };

        _identityServiceMock
            .Setup(x => x.AuthenticateAsync(command.UserNameOrEmail, command.Password))
            .ReturnsAsync((Result.Failure(new[] { "Invalid username/email or password." }), null));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
        result.Errors.ShouldContain("Invalid username/email or password.");
        result.Value.ShouldBeNull();
    }
}
