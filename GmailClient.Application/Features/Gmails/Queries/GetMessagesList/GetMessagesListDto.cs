namespace GmailClient.Application.Features.Gmails.Queries.GetMessagesList
{
    public class GetMessagesListDto
    {
        public string Subject { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsSent { get; set; }
        public bool IsInbox { get; set; }
    }
}
