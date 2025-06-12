using MediatR;

namespace GmailClient.Application.Features.Drafts.Commands.CreateDraft
{
    public class CreateDraftCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
