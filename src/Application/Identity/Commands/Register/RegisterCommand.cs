using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Identity.Commands.Register;

public record RegisterCommand : IRequest<Result<AuthResponse>>
{
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (result, authResponse) = await _identityService.RegisterAsync(request.UserName, request.Email, request.Password);

        if (!result.Succeeded || authResponse == null)
        {
            return Result<AuthResponse>.Failure(result.Errors);
        }

        return Result<AuthResponse>.Success(authResponse);
    }
}
