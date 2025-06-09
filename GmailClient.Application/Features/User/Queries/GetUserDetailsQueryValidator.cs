using FluentValidation;

namespace GmailClient.Application.Features.User.Queries
{
    public class GetUserDetailsQueryValidator : AbstractValidator<GetUserDetailsQuery>
    {
        public GetUserDetailsQueryValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty()
                .NotNull().WithMessage("User not found");
        }
    }
}
