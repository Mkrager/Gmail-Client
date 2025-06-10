using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Application.Contracts.Services;

namespace GmailClient.Application.Services
{
    public class AccessTokenManager : IAccessTokenManager
    {
        private readonly IUserGmailTokenRepository _userGmailTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenEncryptionService _tokenEncryptionService;

        public AccessTokenManager(IUserGmailTokenRepository userGmailTokenRepository, ITokenService tokenService, ITokenEncryptionService tokenEncryptionService)
        {
            _userGmailTokenRepository = userGmailTokenRepository;
            _tokenService = tokenService;
            _tokenEncryptionService = tokenEncryptionService;
        }

        public async Task<string> GetValidAccessTokenAsync(string userId)
        {
            var tokenEntity = (await _userGmailTokenRepository.ListAsync(x => x.UserId == userId)).FirstOrDefault();

            var decryptedToken = _tokenEncryptionService.Decrypt(tokenEntity.RefreshToken);

            if (tokenEntity == null)
                throw new Exception("Token not found");

            if (tokenEntity.ExpiresAt <= DateTime.UtcNow)
            {
                var newToken = await _tokenService.GetAccessTokenAsync(decryptedToken);

                if (newToken.AccessToken == null)
                    throw new Exception("Failed to refresh access token");

                await _userGmailTokenRepository.UpdateAccessTokenAsync(userId, newToken.AccessToken, newToken.ExpiresAt);

                return newToken.AccessToken;
            }

            return tokenEntity.AccessToken;
        }
    }
}
