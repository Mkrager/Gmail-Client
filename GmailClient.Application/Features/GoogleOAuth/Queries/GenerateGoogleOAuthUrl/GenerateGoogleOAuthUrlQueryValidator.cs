using FluentValidation;
using GmailClient.Application.Contracts.Identity;

namespace GmailClient.Application.Features.GoogleOAuth.Queries.GenerateGoogleOAuthUrl
{
    public class GenerateGoogleOAuthUrlQueryValidator : AbstractValidator<GenerateGoogleOAuthUrlQuery>
    {
        private readonly IUserService _userService;
        public GenerateGoogleOAuthUrlQueryValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(r => r)
                .MustAsync(IsGoogleConnected)
                .WithMessage("User already connected google account");
        }
        private async Task<bool> IsGoogleConnected(GenerateGoogleOAuthUrlQuery e, CancellationToken cancellationToken)
        {
            return !(await _userService.IsGoogleConnected(e.UserId));
        }
    }
}
