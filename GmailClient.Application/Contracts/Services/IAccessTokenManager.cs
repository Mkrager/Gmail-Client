namespace GmailClient.Application.Contracts.Services
{
    public interface IAccessTokenManager
    {
        Task<string> GetValidAccessTokenAsync(string userId);
    }
}
