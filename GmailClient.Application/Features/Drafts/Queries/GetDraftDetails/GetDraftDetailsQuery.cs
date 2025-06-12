using MediatR;

namespace GmailClient.Application.Features.Drafts.Queries.GetDraftDetails
{
    public class GetDraftDetailsQuery : IRequest<GetDraftDetailsVm>
    {
        public string UserId { get; set; } = string.Empty;
        public string DraftId { get; set; } = string.Empty;
    }
}
