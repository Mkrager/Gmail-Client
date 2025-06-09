namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListVm
    {
        public List<GetMessagesListDto> Messages { get; set; } = default!;
        public string NextPageToken { get; set; } = string.Empty;
    }
}
