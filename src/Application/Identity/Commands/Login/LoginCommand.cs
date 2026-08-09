using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Identity.Commands.Login;

public record LoginCommand : IRequest<Result<AuthResponse>>
{
    public string UserNameOrEmail { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (result, authResponse) = await _identityService.AuthenticateAsync(request.UserNameOrEmail, request.Password);

        if (!result.Succeeded || authResponse == null)
        {
            return Result<AuthResponse>.Failure(result.Errors);
        }

        return Result<AuthResponse>.Success(authResponse);
    }
}
