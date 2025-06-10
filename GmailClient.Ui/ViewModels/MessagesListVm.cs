namespace GmailClient.Ui.ViewModels
{
    public class MessagesListVm
    {
        public List<MessagesListDto> Messages { get; set; } = default!;
        public string NextPageToken { get; set; } = string.Empty;
    }
}
