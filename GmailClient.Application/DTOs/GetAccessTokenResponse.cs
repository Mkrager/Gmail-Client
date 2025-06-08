namespace GmailClient.Application.DTOs
{
    public class GetAccessTokenResponse
    {
        public DateTime ExpiresAt { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
