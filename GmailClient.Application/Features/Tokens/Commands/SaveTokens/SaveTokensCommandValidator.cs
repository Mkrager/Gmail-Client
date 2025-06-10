using FluentValidation;
using GmailClient.Application.Contracts.Identity;

namespace GmailClient.Application.Features.Tokens.Commands.SaveTokens
{
    public class SaveTokensCommandValidator : AbstractValidator<SaveTokensCommand>
    {
        private readonly IUserService _userService;
        public SaveTokensCommandValidator(IUserService userService)
        {
            _userService = userService;

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

            RuleFor(r => r)
                .MustAsync(IsGoogleConnected)
                .WithMessage("User already connected google account");
        }

        private async Task<bool> IsGoogleConnected(SaveTokensCommand e, CancellationToken cancellationToken)
        {
            return !(await _userService.IsGoogleConnected(e.UserId));
        }
    }
}
