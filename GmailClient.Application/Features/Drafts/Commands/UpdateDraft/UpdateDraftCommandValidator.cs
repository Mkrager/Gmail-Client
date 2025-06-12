using FluentValidation;

namespace GmailClient.Application.Features.Drafts.Commands.UpdateDraft
{
    public class UpdateDraftCommandValidator : AbstractValidator<UpdateDraftCommand>
    {
        public UpdateDraftCommandValidator()
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

            RuleFor(r => r.DraftId)
                .NotEmpty()
                .NotNull().WithMessage("Draft not found");
        }
    }
}
