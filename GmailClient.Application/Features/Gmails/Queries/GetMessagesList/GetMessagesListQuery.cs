using MediatR;

namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListQuery : IRequest<List<GetMessagesListVm>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
