namespace GmailClient.Application.Features.Drafts.Queries.GetDraftDetails
{
    public class GetDraftDetailsVm
    {
        public string DraftId { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
