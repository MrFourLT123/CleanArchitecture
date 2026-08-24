using FluentValidation;

namespace CleanArchitecture.Application.WordProgresses.Commands.SyncWordProgress;

public class SyncWordProgressCommandValidator : AbstractValidator<SyncWordProgressCommand>
{
    public SyncWordProgressCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(v => v.WordId)
            .GreaterThan(0).WithMessage("WordId must be greater than 0.");
            
        RuleFor(v => v.Word)
            .NotEmpty().WithMessage("Word is required.");
    }
}
