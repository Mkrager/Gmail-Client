using MediatR;

namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListQuery : IRequest<GetMessagesListVm>
    {
        public string UserId { get; set; } = string.Empty;
        public string NextPageToken { get; set; } = string.Empty;
    }
}
