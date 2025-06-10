using GmailClient.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.DataProtection;

namespace GmailClient.Infrastructure.Token
{
    public class TokenEncryptionService : ITokenEncryptionService
    {
        private readonly IDataProtector _protector;

        public TokenEncryptionService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("GoogleRefreshTokenProtector");
        }

        public string Encrypt(string token) => _protector.Protect(token);

        public string Decrypt(string encryptedToken) => _protector.Unprotect(encryptedToken);
    }
}
