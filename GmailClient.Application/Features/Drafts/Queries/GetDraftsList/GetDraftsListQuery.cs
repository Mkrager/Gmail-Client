using MediatR;

namespace GmailClient.Application.Features.Drafts.Queries.GetDraftsList
{
    public class GetDraftsListQuery : IRequest<List<GetDraftsListVm>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
