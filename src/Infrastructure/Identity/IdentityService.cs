using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IHttpClientFactory _httpClientFactory;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IJwtTokenGenerator jwtTokenGenerator,
        IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<List<UserDetailsDto>> GetUsersAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
    {
        var idList = userIds.Distinct().ToList();
        return await _userManager.Users
            .Where(u => idList.Contains(u.Id))
            .Select(u => new UserDetailsDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Name = u.Name,
                AvatarUrl = u.AvatarUrl
            })
            .ToListAsync(cancellationToken);
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
        if (string.IsNullOrWhiteSpace(userNameOrEmail) || string.IsNullOrWhiteSpace(password))
        {
            return (Result.Failure(new[] { "Invalid username/email or password." }), null);
        }

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
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return (Result.Failure(new[] { "Username, email, and password are required." }), null);
        }

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

    public async Task<(Result Result, AuthResponse? AuthResponse)> GoogleLoginAsync(string idToken, string? email, string? name)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            return (Result.Failure(new[] { "Google ID token is required." }), null);
        }

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        }
        catch (InvalidJwtException ex)
        {
            return (Result.Failure(new[] { $"Invalid Google token: {ex.Message}" }), null);
        }

        var resolvedEmail = payload.Email ?? email;
        if (string.IsNullOrWhiteSpace(resolvedEmail))
        {
            return (Result.Failure(new[] { "Could not retrieve email from Google token." }), null);
        }

        var user = await _userManager.FindByEmailAsync(resolvedEmail);
        if (user == null)
        {
            var resolvedName = payload.Name ?? name ?? resolvedEmail;
            var userName = resolvedEmail.Split('@')[0] + "_" + Guid.NewGuid().ToString("N")[..6];
            user = new ApplicationUser
            {
                UserName = userName,
                Email = resolvedEmail,
                Name = resolvedName,
                EmailConfirmed = true
            };
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                return (createResult.ToApplicationResult(), null);
            }
        }

        return await BuildAuthResponseAsync(user);
    }

    public async Task<(Result Result, AuthResponse? AuthResponse)> AppleLoginAsync(string identityToken, string? email, string? fullName)
    {
        if (string.IsNullOrWhiteSpace(identityToken))
        {
            return (Result.Failure(new[] { "Apple identity token is required." }), null);
        }

        string? resolvedEmail;
        try
        {
            // Fetch Apple's public keys
            var httpClient = _httpClientFactory.CreateClient();
            var jwks = await httpClient.GetFromJsonAsync<AppleJwks>("https://appleid.apple.com/auth/keys");
            if (jwks == null)
            {
                return (Result.Failure(new[] { "Failed to retrieve Apple public keys." }), null);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://appleid.apple.com",
                ValidateAudience = false,   // Set ValidAudience = your Apple App ID for production
                ValidateLifetime = true,
                IssuerSigningKeys = jwks.Keys.Select(k =>
                {
                    var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                    rsa.ImportParameters(new System.Security.Cryptography.RSAParameters
                    {
                        Modulus = Base64UrlEncoder.DecodeBytes(k.N),
                        Exponent = Base64UrlEncoder.DecodeBytes(k.E)
                    });
                    return (SecurityKey)new RsaSecurityKey(rsa) { KeyId = k.Kid };
                })
            };

            tokenHandler.ValidateToken(identityToken, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            resolvedEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value
                            ?? email;
        }
        catch (Exception ex)
        {
            return (Result.Failure(new[] { $"Invalid Apple token: {ex.Message}" }), null);
        }

        if (string.IsNullOrWhiteSpace(resolvedEmail))
        {
            return (Result.Failure(new[] { "Could not retrieve email from Apple token. Please provide an email." }), null);
        }

        var user = await _userManager.FindByEmailAsync(resolvedEmail);
        if (user == null)
        {
            var userName = resolvedEmail.Split('@')[0] + "_" + Guid.NewGuid().ToString("N")[..6];
            user = new ApplicationUser
            {
                UserName = userName,
                Email = resolvedEmail,
                Name = fullName,
                EmailConfirmed = true
            };
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                return (createResult.ToApplicationResult(), null);
            }
        }

        return await BuildAuthResponseAsync(user);
    }

    private async Task<(Result Result, AuthResponse? AuthResponse)> BuildAuthResponseAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiration) = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            roles);

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

// Models for deserializing Apple's public JWKS endpoint
internal sealed class AppleJwks
{
    [JsonPropertyName("keys")]
    public List<AppleJwk> Keys { get; set; } = new();
}

internal sealed class AppleJwk
{
    [JsonPropertyName("kty")]
    public string Kty { get; set; } = string.Empty;

    [JsonPropertyName("kid")]
    public string Kid { get; set; } = string.Empty;

    [JsonPropertyName("use")]
    public string Use { get; set; } = string.Empty;

    [JsonPropertyName("alg")]
    public string Alg { get; set; } = string.Empty;

    [JsonPropertyName("n")]
    public string N { get; set; } = string.Empty;

    [JsonPropertyName("e")]
    public string E { get; set; } = string.Empty;
}

