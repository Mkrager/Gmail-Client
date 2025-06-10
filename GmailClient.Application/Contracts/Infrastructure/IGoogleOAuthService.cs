using GmailClient.Application.DTOs;

namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface IGoogleOAuthService
    {
        Task<GoogleTokenResponse> ExchangeCodeForTokensAsync(string code, string redirectUri);
        string GenerateGoogleAuthorizationUrl(string state);
    }
}
