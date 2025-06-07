namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        Task<string?> GetAccessTokenAsync(string refreshToken);
    }
}
