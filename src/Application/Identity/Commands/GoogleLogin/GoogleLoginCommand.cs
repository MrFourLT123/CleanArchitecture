using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Identity.Commands.GoogleLogin;

public record GoogleLoginCommand : IRequest<Result<AuthResponse>>
{
    public string IdToken { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Name { get; init; }
}

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, Result<AuthResponse>>
{
    private readonly IIdentityService _identityService;

    public GoogleLoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<AuthResponse>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var (result, authResponse) = await _identityService.GoogleLoginAsync(
            request.IdToken,
            request.Email,
            request.Name);

        if (!result.Succeeded || authResponse == null)
        {
            return Result<AuthResponse>.Failure(result.Errors);
        }

        return Result<AuthResponse>.Success(authResponse);
    }
}
