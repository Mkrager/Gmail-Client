using FluentValidation;

namespace GmailClient.Application.Features.Tokens.Commands.SaveTokens
{
    public class SaveTokensCommandValidator : AbstractValidator<SaveTokensCommand>
    {
        public SaveTokensCommandValidator()
        {
            RuleFor(r => r.UserId)
                .NotNull()
                .NotEmpty().WithMessage("UserNotFound");

            RuleFor(r => r.RefreshToken)
                .NotEmpty()
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(r => r.AccessToken)
                .NotEmpty()
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(r => r.ExpiresAt)
                .GreaterThan(0).WithMessage("{PropertyName should be positiv value}")
                .NotEmpty()
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
