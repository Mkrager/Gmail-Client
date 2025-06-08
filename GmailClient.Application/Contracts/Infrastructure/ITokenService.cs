using GmailClient.Application.DTOs;

namespace GmailClient.Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        Task<GetAccessTokenResponse> GetAccessTokenAsync(string refreshToken);
    }
}
