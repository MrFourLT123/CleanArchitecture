using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<(Result Result, AuthResponse? AuthResponse)> AuthenticateAsync(string userNameOrEmail, string password)
    {
        var user = await _userManager.FindByEmailAsync(userNameOrEmail)
                   ?? await _userManager.FindByNameAsync(userNameOrEmail);

        if (user == null)
        {
            return (Result.Failure(new[] { "Invalid username/email or password." }), null);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            return (Result.Failure(new[] { "Invalid username/email or password." }), null);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var (token, expiration) = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty, roles);

        var authResponse = new AuthResponse
        {
            Token = token,
            Expiration = expiration,
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = roles
        };

        return (Result.Success(), authResponse);
    }

    public async Task<(Result Result, AuthResponse? AuthResponse)> RegisterAsync(string userName, string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email)
                           ?? await _userManager.FindByNameAsync(userName);

        if (existingUser != null)
        {
            return (Result.Failure(new[] { "User with this username or email already exists." }), null);
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return (result.ToApplicationResult(), null);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiration) = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email, roles);

        var authResponse = new AuthResponse
        {
            Token = token,
            Expiration = expiration,
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles
        };

        return (Result.Success(), authResponse);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}
