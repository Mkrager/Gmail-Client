namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface IGoogleOAuthStateService
    {
        void StoreState(string state, string userId, TimeSpan expiration);
        bool TryGetUserIdByState(string state, out string userId);
    }
}
