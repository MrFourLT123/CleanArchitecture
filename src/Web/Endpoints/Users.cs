using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Identity.Commands.AppleLogin;
using CleanArchitecture.Application.Identity.Commands.GoogleLogin;
using CleanArchitecture.Application.Identity.Commands.Login;
using CleanArchitecture.Application.Identity.Commands.Register;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Endpoints;

public class Users : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(Login, "login").AllowAnonymous();
        groupBuilder.MapPost(Register, "register").AllowAnonymous();
        groupBuilder.MapPost(GoogleLogin, "google-login").AllowAnonymous();
        groupBuilder.MapPost(AppleLogin, "apple-login").AllowAnonymous();
        groupBuilder.MapGet(GetMe, "me");
    }

    [EndpointSummary("User Login")]
    [EndpointDescription("Authenticates user credentials and returns a JWT access token.")]
    public static async Task<Results<Ok<AuthResponse>, BadRequest<string[]>>> Login(ISender sender, [FromBody] LoginCommand command)
    {
        var result = await sender.Send(command);

        if (!result.Succeeded || result.Value == null)
        {
            return TypedResults.BadRequest(result.Errors);
        }

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("User Registration")]
    [EndpointDescription("Registers a new user account and returns a JWT access token.")]
    public static async Task<Results<Ok<AuthResponse>, BadRequest<string[]>>> Register(ISender sender, [FromBody] RegisterCommand command)
    {
        var result = await sender.Send(command);

        if (!result.Succeeded || result.Value == null)
        {
            return TypedResults.BadRequest(result.Errors);
        }

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Google Login")]
    [EndpointDescription("Authenticates a user via a Google ID token and returns a JWT access token. Auto-creates the account on first login.")]
    public static async Task<Results<Ok<AuthResponse>, BadRequest<string[]>>> GoogleLogin(ISender sender, [FromBody] GoogleLoginCommand command)
    {
        var result = await sender.Send(command);

        if (!result.Succeeded || result.Value == null)
        {
            return TypedResults.BadRequest(result.Errors);
        }

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Apple Login")]
    [EndpointDescription("Authenticates a user via an Apple identity token and returns a JWT access token. Auto-creates the account on first login.")]
    public static async Task<Results<Ok<AuthResponse>, BadRequest<string[]>>> AppleLogin(ISender sender, [FromBody] AppleLoginCommand command)
    {
        var result = await sender.Send(command);

        if (!result.Succeeded || result.Value == null)
        {
            return TypedResults.BadRequest(result.Errors);
        }

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Get Current User Profile")]
    [EndpointDescription("Returns profile and role information for the currently authenticated user.")]
    public static async Task<Results<Ok<CurrentUserDto>, UnauthorizedHttpResult>> GetMe(IUser user, IIdentityService identityService)
    {
        if (string.IsNullOrEmpty(user.Id))
        {
            return TypedResults.Unauthorized();
        }

        var userName = await identityService.GetUserNameAsync(user.Id);

        var dto = new CurrentUserDto
        {
            Id = user.Id,
            UserName = userName ?? string.Empty,
            Roles = user.Roles ?? new List<string>()
        };

        return TypedResults.Ok(dto);
    }
}

public class CurrentUserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
}
