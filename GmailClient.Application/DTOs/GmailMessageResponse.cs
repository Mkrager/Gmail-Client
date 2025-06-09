namespace GmailClient.Application.DTOs
{
    public class GmailMessageResponse
    {
        public List<GmailMessageDto> Messages { get; set; } = new();
        public string NextPageToken { get; set; } = string.Empty;
    }
}
