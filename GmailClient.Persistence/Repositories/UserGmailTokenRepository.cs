using GmailClient.Application.Contracts.Persistance;
using GmailClient.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GmailClient.Persistence.Repositories
{
    public class UserGmailTokenRepository : BaseRepository<UserGmailToken>, IUserGmailTokenRepository
    {
        public UserGmailTokenRepository(GmailClientDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserGmailToken?> GetByUserIdAsync(string userId)
        {
            return await _dbContext.UserGmailTokens
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateAccessTokenAsync(UserGmailToken user, string newAccessToken, DateTime expiresAt)
        {
            user.AccessToken = newAccessToken;
            user.ExpiresAt = expiresAt;

            await _dbContext.SaveChangesAsync();
        }
    }
}
