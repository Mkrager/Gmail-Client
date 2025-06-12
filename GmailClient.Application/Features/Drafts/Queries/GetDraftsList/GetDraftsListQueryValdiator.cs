using FluentValidation;

namespace GmailClient.Application.Features.Drafts.Queries.GetDraftsList
{
    public class GetDraftsListQueryValdiator : AbstractValidator<GetDraftsListQuery>
    {
        public GetDraftsListQueryValdiator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .NotNull().WithMessage("User not found");
        }
    }
}
