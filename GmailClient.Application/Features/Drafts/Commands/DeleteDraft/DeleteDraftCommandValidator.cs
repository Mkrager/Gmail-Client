using FluentValidation;

namespace GmailClient.Application.Features.Drafts.Commands.DeleteDraft
{
    public class DeleteDraftCommandValidator : AbstractValidator<DeleteDraftCommand>
    {
        public DeleteDraftCommandValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .NotNull().WithMessage("User not founed");
        }
    }
}
