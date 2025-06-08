using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Contracts.Services;

namespace GmailClient.Application.Services
{
    public class AccessTokenManager : IAccessTokenManager
    {
        private readonly IUserGmailTokenRepository _userGmailTokenRepository;
        private readonly ITokenService _tokenService;

        public AccessTokenManager(IUserGmailTokenRepository userGmailTokenRepository, ITokenService tokenService)
        {
            _userGmailTokenRepository = userGmailTokenRepository;
            _tokenService = tokenService;
        }

        public async Task<string> GetValidAccessTokenAsync(string userId)
        {
            var tokenEntity = (await _userGmailTokenRepository.ListAsync(x => x.UserId == userId)).FirstOrDefault();

            if (tokenEntity == null)
                throw new Exception("Token not found");

            if (tokenEntity.ExpiresAt <= DateTime.UtcNow)
            {
                var newToken = await _tokenService.GetAccessTokenAsync(tokenEntity.RefreshToken);

                if (newToken.AccessToken == null)
                    throw new Exception("Failed to refresh access token");

                await _userGmailTokenRepository.UpdateAccessTokenAsync(userId, newToken.AccessToken, newToken.ExpiresAt);

                return newToken.AccessToken;
            }

            return tokenEntity.AccessToken;
        }
    }
}
