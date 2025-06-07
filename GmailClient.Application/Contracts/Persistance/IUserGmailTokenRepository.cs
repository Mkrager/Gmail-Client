using GmailClient.Domain.Entities;

namespace GmailClient.Application.Contracts.Persistance
{
    public interface IUserGmailTokenRepository : IAsyncRepository<UserGmailToken>
    {
        Task UpdateAccessTokenAsync(string userId, string newAccessToken, DateTime expiresAt);
    }
}
