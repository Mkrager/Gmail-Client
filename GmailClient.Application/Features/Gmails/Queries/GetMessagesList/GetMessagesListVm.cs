namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListVm
    {
        public string Id { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }
}
