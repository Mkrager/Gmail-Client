namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        Task<(string AccessToken, DateTime ExpiresAt)> GetAccessTokenAsync(string refreshToken);
    }
}
