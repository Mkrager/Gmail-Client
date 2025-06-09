using FluentValidation;

namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListQueryValidator : AbstractValidator<GetMessagesListQuery>
    {
        public GetMessagesListQueryValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .NotEmpty().WithMessage("User not found");
        }
    }
}
