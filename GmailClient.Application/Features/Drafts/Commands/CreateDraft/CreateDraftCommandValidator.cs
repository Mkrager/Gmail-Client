using FluentValidation;

namespace GmailClient.Application.Features.Drafts.Commands.CreateDraft
{
    public class CreateDraftCommandValidator : AbstractValidator<CreateDraftCommand>
    {
        public CreateDraftCommandValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .NotNull().WithMessage("User not founed");

            RuleFor(r => r.Subject)
                .NotEmpty()
                .NotNull().WithMessage("Subject required");

            RuleFor(r => r.Body)
                .NotEmpty()
                .NotNull().WithMessage("Body required");

            RuleFor(r => r.To)
                .NotEmpty()
                .NotNull().WithMessage("To required");
        }
    }
}
