namespace CleanArchitecture.Application.Phrases1000s.Queries;

public class GetPhrases1000sQueryValidator : AbstractValidator<GetPhrases1000sQuery>
{
    public GetPhrases1000sQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
