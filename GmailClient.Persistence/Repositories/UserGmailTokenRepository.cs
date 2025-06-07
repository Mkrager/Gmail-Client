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
        public async Task UpdateAccessTokenAsync(string userId, string newAccessToken, DateTime expiresAt)
        {
            var user = await _dbContext.UserGmailTokens.FirstOrDefaultAsync(u => u.UserId == userId); 

            if (user is null)
                throw new Exception("User not found");

            user.AccessToken = newAccessToken;
            user.ExpiresAt = expiresAt;

            await _dbContext.SaveChangesAsync();
        }
    }
}
