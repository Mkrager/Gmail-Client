using FluentValidation;

namespace GmailClient.Application.Features.Gmails.Commands.SendEmail
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(r => r.Body)
                .NotEmpty()
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(r => r.Subject)
                .NotEmpty()
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(r => r.To)
                .NotEmpty()
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
