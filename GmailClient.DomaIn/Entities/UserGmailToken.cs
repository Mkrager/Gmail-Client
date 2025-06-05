namespace GmailClient.Domain.Entities
{
    public class UserGmailToken
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
