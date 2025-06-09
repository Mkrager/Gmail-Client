using GmailClient.Application.DTOs;

namespace GmailClient.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<UserDetailsResponse> GetUserDetails(string userId);
        Task SetGoogleConnectedAsync(string userId, bool isConnected);
    }
}
