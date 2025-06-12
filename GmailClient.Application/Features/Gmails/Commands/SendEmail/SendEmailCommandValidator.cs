using FluentValidation;

namespace GmailClient.Application.Features.Gmails.Commands.SendEmail
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(r => r.Body)
                .NotEmpty()
                .NotEmpty().WithMessage("Body is required.");

            RuleFor(r => r.Subject)
                .NotEmpty()
                .NotEmpty().WithMessage("Subject is required.");

            RuleFor(r => r.To)
                .NotEmpty()
                .NotEmpty().WithMessage("To is required.");
            RuleFor(r => r.UserId)
                .NotEmpty()
                .NotEmpty().WithMessage("User not found");
        }
    }
}
