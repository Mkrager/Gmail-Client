using FluentValidation;

namespace GmailClient.Application.Features.Drafts.Queries.GetDraftDetails
{
    public class GetDraftDetailsQueryValidator : AbstractValidator<GetDraftDetailsQuery>
    {
        public GetDraftDetailsQueryValidator()
        {
            RuleFor(r => r.UserId)
                .NotNull()
                .NotEmpty().WithMessage("User not found");

            RuleFor(r => r.DraftId)
                .NotEmpty()
                .NotNull().WithMessage("Draft not found");
        }
    }
}
