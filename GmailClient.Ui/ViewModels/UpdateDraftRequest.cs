namespace GmailClient.Ui.ViewModels
{
    public class UpdateDraftRequest
    {
        public string DraftId { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
