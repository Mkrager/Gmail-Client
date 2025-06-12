using MediatR;

namespace GmailClient.Application.Features.Drafts.Commands.DeleteDraft
{
    public class DeleteDraftCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string DraftId { get; set; } = string.Empty;
    }
}
