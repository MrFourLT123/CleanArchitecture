using CleanArchitecture.Application.Identity.Commands.Login;
using NUnit.Framework;
using Shouldly;

namespace CleanArchitecture.Application.UnitTests.Identity;

[TestFixture]
public class LoginCommandValidatorTests
{
    private LoginCommandValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new LoginCommandValidator();
    }

    [Test]
    public void Validate_EmptyUserNameOrEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            UserNameOrEmail = "",
            Password = "Password123!"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(LoginCommand.UserNameOrEmail));
    }

    [Test]
    public void Validate_EmptyPassword_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            UserNameOrEmail = "user@example.com",
            Password = ""
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(LoginCommand.Password));
    }

    [Test]
    public void Validate_ValidCommand_ShouldBeValid()
    {
        // Arrange
        var command = new LoginCommand
        {
            UserNameOrEmail = "user@example.com",
            Password = "Password123!"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
