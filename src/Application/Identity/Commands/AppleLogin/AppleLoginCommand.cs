using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Identity.Commands.AppleLogin;

public record AppleLoginCommand : IRequest<Result<AuthResponse>>
{
    public string IdentityToken { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? FullName { get; init; }
}

public class AppleLoginCommandHandler : IRequestHandler<AppleLoginCommand, Result<AuthResponse>>
{
    private readonly IIdentityService _identityService;

    public AppleLoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<AuthResponse>> Handle(AppleLoginCommand request, CancellationToken cancellationToken)
    {
        var (result, authResponse) = await _identityService.AppleLoginAsync(
            request.IdentityToken,
            request.Email,
            request.FullName);

        if (!result.Succeeded || authResponse == null)
        {
            return Result<AuthResponse>.Failure(result.Errors);
        }

        return Result<AuthResponse>.Success(authResponse);
    }
}
